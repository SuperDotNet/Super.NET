using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Text;

namespace Super.Serialization
{
	public sealed class DefaultEncoding : Instance<Encoding>
	{
		public static DefaultEncoding Default { get; } = new DefaultEncoding();

		DefaultEncoding() : base(new UTF8Encoding(false, true)) {}
	}

	public sealed class Encoder : Select<byte[], string>, ISelect<string, byte[]>
	{
		readonly Func<string, byte[]> _parse;
		public static Encoder Default { get; } = new Encoder();

		Encoder() : this(DefaultEncoding.Default) {}

		public Encoder(Encoding encoding) : this(encoding.GetString, encoding.GetBytes) {}

		public Encoder(Func<byte[], string> format, Func<string, byte[]> parse) : base(format) => _parse = parse;

		public byte[] Get(string parameter) => _parse(parameter);
	}

	public interface IWriter<in T> : ISelect<T, Array<byte>> {}

	class Writer<T> : IWriter<T>
	{
		readonly IEmit<T>        _emitter;
		readonly ArrayPool<byte> _pool;
		readonly uint            _size;

		public Writer(IEmit<T> emitter) : this(emitter, DefaultBufferSize.Default) {}

		public Writer(IEmit<T> emitter, uint size) : this(emitter, ArrayPool<byte>.Shared, size) {}

		public Writer(IEmit<T> emitter, ArrayPool<byte> pool, uint size)
		{
			_emitter = emitter;
			_pool    = pool;
			_size    = size;
		}

		public Array<byte> Get(T parameter)
		{
			var composition = _emitter.Get(new Composition<T>(_pool.Rent((int)_size), parameter));
			var result      = composition.Output.CopyInto(new byte[composition.Index], 0, composition.Index);
			composition.Output.Clear(composition.Index);
			_pool.Return(composition.Output);
			return result;
		}
	}

	public class SingleInstructionWriter<T> : IWriter<T>
	{
		readonly IInstruction<T> _instruction;
		readonly ArrayPool<byte> _pool;

		public SingleInstructionWriter(IInstruction<T> instruction)
			: this(instruction, ArrayPool<byte>.Shared) {}

		public SingleInstructionWriter(IInstruction<T> instruction, ArrayPool<byte> pool)
		{
			_instruction = instruction;
			_pool        = pool;
		}

		public Array<byte> Get(T parameter)
		{
			var source = _pool.Rent((int)_instruction.Get(parameter));
			var length = _instruction.Get(new Composition<T>(source, parameter));
			var result = source.CopyInto(new byte[length], 0, length);
			source.Clear(length);
			_pool.Return(source);
			return result;
		}
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

	public readonly struct Composition<T>
	{
		public static implicit operator Composition(Composition<T> instance)
			=> new Composition(instance.Output, instance.Index);

		public Composition(byte[] output, in T instance, in uint index = 0)
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