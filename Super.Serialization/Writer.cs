using Super.Model;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;

namespace Super.Serialization
{
	public interface IWriter<in T> : ISelect<T, Array<byte>> {}

	sealed class NumberWriter : Writer<uint>
	{
		public static NumberWriter Default { get; } = new NumberWriter();

		NumberWriter() : base(NumberComposer.Default) {}
	}

	class Writer<T> : IWriter<T>
	{
		readonly IComposer<T>   _writer;
		readonly IStorage<byte> _storage;

		public Writer(IComposer<T> writer) : this(writer, Leases<byte>.Default) {}

		public Writer(IComposer<T> writer, IStorage<byte> storage)
		{
			_writer  = writer;
			_storage = storage;
		}

		public Array<byte> Get(T parameter)
		{
			var store = _storage.Get(16);
			using (new Session<byte>(store, _storage))
			{
				var composition = _writer.Get(new Composition<T>(new Composition(store.Instance), parameter));
				var result      = new ArrayView<byte>(store.Instance, 0, composition.Index).ToArray();
				return result;
			}
		}
	}

	/*class Writer<T> : IWriter<T>
	{
		readonly IComposer<T> _composer;

		public Writer(IComposer<T> composer) => _composer = composer;

		public void Execute(Writing<T> parameter)
		{
			_composer.Get(new Composition<T>(parameter.Composition, parameter.Instance));
		}
	}*/

	public interface IComposer<T> : ISelect<Composition<T>, Composition> {}

	sealed class NumberComposer : IComposer<uint>
	{
		public static NumberComposer Default { get; } = new NumberComposer();

		NumberComposer() {}

		public Composition Get(Composition<uint> parameter)
		{
			var current = parameter.Current;
			var result = Utf8Formatter.TryFormat(parameter.Instance, current.Output.AsSpan().Slice((int)current.Index),
			                                     out var count)
				             ? new Composition(current.Output, (uint)(current.Index + count))
				             : current;
			return result;
		}
	}

	/*public interface IOutput : IBufferWriter<byte> {}*/

	public readonly struct Composition<T>
	{
		public Composition(Composition composition, T instance)
		{
			Current  = composition;
			Instance = instance;
		}

		public Composition Current { get; }

		public T Instance { get; }
	}

	public readonly struct Composition
	{
		public Composition(byte[] output, uint index = 0)
		{
			Output = output;
			Index  = index;
		}

		public byte[] Output { get; }
		public uint Index { get; }
	}

	/*public readonly struct Writing<T>
	{
		public Writing(Composition composition, T instance)
		{
			Composition = composition;
			Instance    = instance;
		}

		public Composition Composition { get; }

		public T Instance { get; }
	}*/

	/*public interface IFormatter : IDisposable
	{
		void Start(Type identity);

		void Complete();
	}*/

	/*public sealed class JsonFormatter : IFormatter
	{
		readonly IBufferWriter<byte> _store;

		public JsonFormatter(IBufferWriter<byte> store) => _store = store;

		public void Dispose() {}

		public void Start(Type identity) {}

		public void Complete() {}
	}*/

	/*public interface IWriter<T> : ICommand<Input<T>> {}

	public readonly struct Input<T>
	{
		public Input(Stream stream, T instance)
		{
			Stream   = stream;
			Instance = instance;
		}

		public Stream Stream { get; }

		public T Instance { get; }
	}*/

	public interface IOutput : ISelect<Assigned<uint>, ArrayView<byte>>,
	                                 IResult<Array<byte>>,
	                                 ICommand<uint>,
	                                 IDisposable
	{
		uint Count { get; }
	}

	sealed class Output : IOutput
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

		public uint Available => (uint)(_store.Length - Count);*/

		void Clear()
		{
			_store.AsSpan(0, (int)Count).Clear();
			Count = 0;
		}

		/*public void Advance(int count)
		{

		}

		public Memory<byte> GetMemory(int sizeHint = 0) => Ensured(sizeHint).AsMemory(WrittenCount);

		public Span<byte> GetSpan(int sizeHint = 0) => Ensured(sizeHint).AsSpan(WrittenCount);*/

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
		}*/

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
	}
}