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

	public sealed class DefaultBufferSize : Instance<uint>
	{
		public static DefaultBufferSize Default { get; } = new DefaultBufferSize();

		DefaultBufferSize() : base(1024 * 16) {}
	}

	class Writer<T> : IWriter<T>
	{
		readonly IEmit<T>       _writer;
		readonly ArrayPool<byte> _pool;
		readonly uint           _size;

		public Writer(IEmit<T> writer) : this(writer, ArrayPool<byte>.Shared, DefaultBufferSize.Default) {}

		public Writer(IEmit<T> writer, ArrayPool<byte> pool, uint size)
		{
			_writer  = writer;
			_pool = pool;
			_size    = size;
		}

		public Array<byte> Get(T parameter) => _writer.Get(new Composition<T>(_pool.Rent((int)_size), parameter))
		                                              .Complete(_pool);
	}

	class Emit<T> : IEmit<T>
	{
		readonly uint                       _size;
		readonly Func<Composition<T>, uint> _emit;

		public Emit(uint size, Func<Composition<T>, uint> emit)
		{
			_size = size;
			_emit = emit;
		}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition<T>(parameter.Output.Copy(in _size), parameter.Instance,
				                                       parameter.Index)
				                  : parameter;
			return new Model.Sequences.Store<byte>(composition.Output, _emit(composition));
		}
	}

	public interface IEmit<T> : ISelect<Composition<T>, Model.Sequences.Store<byte>> {}

	sealed class PositiveNumber : Emit<uint>
	{
		public static PositiveNumber Default { get; } = new PositiveNumber();

		PositiveNumber()
			: base(20,
			       x => Utf8Formatter.TryFormat(x.Instance, x.Output.AsSpan((int)x.Index), out var count)
				            ? (uint)count
				            : throw new
					              InvalidOperationException($"Could not format '{x.Instance}' into its UTF8 equivalent.")) {}
	}

	static class Extensions
	{
		readonly static ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

		readonly static Func<int, byte[]> Rent = Pool.Rent;

		readonly static Action<byte[], bool> Return = Pool.Return;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] Copy(this byte[] @this, in uint size)
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
}