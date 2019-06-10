using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace Super.Serialization
{
	public interface IWriter<in T> : ISelect<T, Array<byte>> {}

	sealed class NumberWriter : Writer<uint>
	{
		public static NumberWriter Default { get; } = new NumberWriter();

		NumberWriter() : base(PositiveNumber.Default) {}
	}

	/*sealed class BodyEmit<T> : IEmit<T>
	{
		readonly IEmit<T> _content;

		public BodyEmit(IEmit<T> content) => _content = content;

		public uint Get(Composition<T> parameter) => 0;
	}*/

	public sealed class DefaultBufferSize : Instance<uint>
	{
		public static DefaultBufferSize Default { get; } = new DefaultBufferSize();

		DefaultBufferSize() : base(1024 * 16) {}
	}

	class Writer<T> : IWriter<T>
	{
		readonly IEmit<T>       _writer;
		readonly IStorage<byte> _storage;
		readonly uint           _size;

		public Writer(IEmit<T> writer) : this(writer, Leases<byte>.Default, DefaultBufferSize.Default) {}

		public Writer(IEmit<T> writer, IStorage<byte> storage, uint size)
		{
			_writer  = writer;
			_storage = storage;
			_size    = size;
		}

		public Array<byte> Get(T parameter)
		{
			var start = new Model.Sequences.Store<byte>(ArrayPool<byte>.Shared.Rent((int)_size), 0);
			using (new Session<byte>(start, _storage))
			{
				return _writer.Get(new Composition<T>(start.Instance, parameter)).Complete();
			}
		}
	}

	class Emit<T> : IEmit<T>
	{
		readonly uint                                              _size;
		readonly Func<Composition<T>, Model.Sequences.Store<byte>> _emit;

		public Emit(uint size, Func<Composition<T>, Model.Sequences.Store<byte>> emit)
		{
			_size = size;
			_emit = emit;
		}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
			=> _emit(parameter.Index + _size >= parameter.Output.Length
				         ? new Composition<T>(parameter.Output.Copy(_size), parameter.Instance, parameter.Index)
				         : parameter);
	}

	public interface IEmit<T> : ISelect<Composition<T>, Model.Sequences.Store<byte>> {}

	sealed class PositiveNumber : Emit<uint>
	{
		public static PositiveNumber Default { get; } = new PositiveNumber();

		PositiveNumber()
			: base(20, x => Utf8Formatter.TryFormat(x.Instance, x.Remaining(), out var count)
				                ? new Model.Sequences.Store<byte>(x.Output, (uint)count)
				                : x.Output) {}
	}

	/*public interface IPrepare : ICommand<ValueTuple<byte[], uint>> {}*/

	/*sealed class Prepare : IPrepare
	{
		readonly uint _size;

		public Prepare(uint size) => _size = size;

		public void Execute((byte[], uint) parameter)
		{

		}
	}*/

	static class Extensions
	{
		readonly static ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

		readonly static Func<int, byte[]> Rent = Pool.Rent;

		readonly static Action<byte[], bool> Return = Pool.Return;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Span<byte> Remaining<T>(this Composition<T> @this) => @this.Output.AsSpan((int)@this.Index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] Copy(this byte[] @this, uint size)
		{
			var result = @this.CopyInto(Rent((int)checked(@this.Length + Math.Max(size, @this.Length))), 0,
			                            (uint)@this.Length);

			Return(@this, false);

			return result;
		}
	}

	public readonly struct Composition<T>
	{
		public Composition(byte[] output, T instance, uint index = 0)
		{
			Output   = output;
			Instance = instance;
			Index    = index;
		}

		public byte[] Output { get; }

		public T Instance { get; }

		public uint Index { get; }
	}

	/*public interface IOutput : ISelect<Assigned<uint>, Model.Sequences.Store<byte>>,
	                           IResult<Array<byte>>,
	                           ICommand<uint>,
	                           IDisposable
	{
		uint Count { get; }
	}

	sealed class Output : IOutput
	{
		const uint Minimum = 32;

		readonly ArrayPool<byte> _pool;
		byte[]                   _store;

		public Output() : this(1024 * 16) {}

		public Output(int initialCapacity) : this(ArrayPool<byte>.Shared, initialCapacity) {}

		public Output(ArrayPool<byte> pool, int initialCapacity) : this(pool, pool.Rent(initialCapacity)) {}

		public Output(ArrayPool<byte> pool, byte[] store)
		{
			_pool  = pool;
			_store = store;
		}

		public uint Count { get; private set; }

		public Model.Sequences.Store<byte> Get(Assigned<uint> parameter)
			=> new Model.Sequences.Store<byte>(Ensured(parameter.Or(Minimum)), Count);

		public Array<byte> Get() => new ArrayView<byte>(_store, 0, Count).ToArray();

		public void Execute(uint parameter)
		{
			Count += parameter;
		}

		/*ValueTask WriteToStreamAsync(Stream destination, CancellationToken cancellationToken)
		{
			return destination.WriteAsync(WrittenMemory, cancellationToken);
		}#1#

		byte[] Ensured(uint requested)
		{
			if (Count + requested >= _store.Length)
			{
				var size = checked(_store.Length + Math.Max(requested, _store.Length));

				var last = _store;

				_store = _pool.Rent((int)size);

				last.AsSpan(0, (int)Count)
				    .CopyTo(_store);
				_pool.Return(last, true);
			}

			return _store;
		}

		public void Dispose()
		{
			if (_store != null)
			{
				//Clear();
				_pool.Return(_store);
				_store = null;
			}
		}
	}*/

	/*sealed class Output : IOutput
	{
		const uint Minimum = 256;

		readonly ArrayPool<byte> _pool;
		byte[]                   _store;

		public Output(int initialCapacity) : this(ArrayPool<byte>.Shared, initialCapacity) {}

		public Output(ArrayPool<byte> pool, int initialCapacity) : this(pool, pool.Rent(initialCapacity)) {}

		public Output(ArrayPool<byte> pool, byte[] store)
		{
			_pool  = pool;
			_store = store;
		}

		public uint Count { get; private set; }

		/*public ReadOnlyMemory<byte> WrittenMemory => _store.AsMemory(0, (int)Count);

		public uint Capacity => (uint)_store.Length;

		public uint Available => (uint)(_store.Length - Count);#1#

		void Clear()
		{
			_store.AsSpan(0, (int)Count).Clear();
			Count = 0;
		}

		/*public void Advance(int count)
		{

		}

		public Memory<byte> GetMemory(int sizeHint = 0) => Ensured(sizeHint).AsMemory(WrittenCount);

		public Span<byte> GetSpan(int sizeHint = 0) => Ensured(sizeHint).AsSpan(WrittenCount);#1#

		public ArrayView<byte> Get(Assigned<uint> parameter)
			=> new ArrayView<byte>(Ensured(parameter.Or(Minimum)), 0, Count);

		public Array<byte> Get() => new ArrayView<byte>(_store, 0, Count).ToArray();

		public void Execute(uint parameter)
		{
			Count += parameter;
		}

		/*ValueTask WriteToStreamAsync(Stream destination, CancellationToken cancellationToken)
		{
			return destination.WriteAsync(WrittenMemory, cancellationToken);
		}#1#

		byte[] Ensured(uint requested)
		{
			if (Count + requested >= _store.Length)
			{
				var size = checked(_store.Length + Math.Max(requested, _store.Length));

				var last = _store;

				_store = _pool.Rent((int)size);

				last.AsSpan(0, (int)Count)
				    .CopyTo(_store);
				_pool.Return(last, true);
			}

			return _store;
		}

		public void Dispose()
		{
			if (_store != null)
			{
				Clear();
				_pool.Return(_store, true);
				_store = null;
			}
		}
	}*/
}