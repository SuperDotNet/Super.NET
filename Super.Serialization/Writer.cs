using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
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

	/*class Writer<T> : IWriter<T>
	{
		readonly ICompose<T>     _emitter;
		readonly ArrayPool<byte> _pool;
		readonly uint            _size;

		public Writer(ICompose<T> emitter) : this(emitter, DefaultBufferSize.Default) {}

		public Writer(ICompose<T> emitter, uint size) : this(emitter, ArrayPool<byte>.Shared, size) {}

		public Writer(ICompose<T> emitter, ArrayPool<byte> pool, uint size)
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
	}*/

	public interface ISessions : ISelect<uint, Session<byte>>, IResult<Session<byte>> {}

	public sealed class Sessions : ISessions
	{
		public static Sessions Default { get; } = new Sessions();

		Sessions() : this(Leases<byte>.Default, DefaultBufferSize.Default.Get) {}

		readonly IStorage<byte> _storage;
		readonly Func<uint>     _size;

		public Sessions(IStorage<byte> storage, Func<uint> size)
		{
			_storage = storage;
			_size    = size;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Session<byte> Get(uint parameter) => _storage.Session(parameter);

		public Session<byte> Get() => Get(_size());
	}

	public class Writer<T> : IWriter<T>
	{
		readonly Array<IInstruction<T>> _instructions;
		readonly ISessions              _sessions;

		public Writer(params IInstruction<T>[] instructions) : this(new Array<IInstruction<T>>(instructions)) {}

		public Writer(Array<IInstruction<T>> instructions) : this(instructions, Sessions.Default) {}

		public Writer(Array<IInstruction<T>> instructions, ISessions sessions)
		{
			_instructions = instructions;
			_sessions     = sessions;
		}

		public Array<byte> Get(T parameter)
		{
			using (var session = _sessions.Get())
			{
				var composition = new Composition<T>(session.Store, parameter);
				var length      = _instructions.Length;
				for (var i = 0u; i < length; i++)
				{
					var instruction = _instructions[i];
					var size        = instruction.Get(parameter);
					composition = composition.Index + size >= composition.Output.Length
									  ? new Composition<T>(composition.Output.Copy(in size), parameter,
														   composition.Index)
									  : composition;

					composition = new Composition<T>(composition.Output, parameter,
													 composition.Index + instruction.Get(composition));
				}

				var result = composition.Output.CopyInto(new byte[composition.Index], 0, composition.Index);
				composition.Output.Clear(composition.Index);
				return result;
			}
		}
	}

	public class SingleInstructionWriter<T> : IWriter<T>
	{
		readonly IInstruction<T>      _instruction;
		readonly Func<int, byte[]>    _lease;
		readonly Action<byte[], bool> _return;

		public SingleInstructionWriter(IInstruction<T> instruction)
			: this(instruction, ArrayPool<byte>.Shared) {}

		public SingleInstructionWriter(IInstruction<T> instruction, ArrayPool<byte> pool)
			: this(instruction, pool.Rent, pool.Return) {}

		public SingleInstructionWriter(IInstruction<T> instruction, Func<int, byte[]> lease,
									   Action<byte[], bool> @return)
		{
			_instruction = instruction;
			_lease       = lease;
			_return      = @return;
		}

		public Array<byte> Get(T parameter)
		{
			var source = _lease((int)_instruction.Get(parameter));
			var length = _instruction.Get(new Composition<T>(source, parameter));
			var result = new byte[length];
			Buffer.BlockCopy(source, 0, result, 0, (int)length);
			source.Clear(length);
			_return(source, false);
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


		public Composition<T> Using(T instance) => new Composition<T>(Output, instance, Index);
	}
}