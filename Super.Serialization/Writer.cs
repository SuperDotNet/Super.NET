using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Serialization
{
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<byte> Complete(this in Composition @this, ArrayPool<byte> pool)
			=> Complete(@this, new byte[@this.Index], pool);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<byte> Complete(in this Composition @this, byte[] into, ArrayPool<byte> pool)
		{
			var result = @this.Output.CopyInto(into, 0u, @this.Index);
			@this.Output.Clear(@this.Index);
			pool.Return(@this.Output);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Composition<T> Introduce<T>(in this Composition @this, in T instance)
			=> new Composition<T>(@this.Output, in instance, @this.Index);
	}

	public sealed class Encoder : Select<byte[], string>, ISelect<string, byte[]>
	{
		readonly Func<string, byte[]> _parse;
		public static Encoder Default { get; } = new Encoder();

		Encoder() : this(new UTF8Encoding(false, true)) {}

		public Encoder(Encoding encoding) : this(encoding.GetString, encoding.GetBytes) {}

		public Encoder(Func<byte[], string> format, Func<string, byte[]> parse) : base(format) => _parse = parse;

		public byte[] Get(string parameter) => _parse(parameter);
	}

	public sealed class DefaultBufferSize : Instance<uint>
	{
		public static DefaultBufferSize Default { get; } = new DefaultBufferSize();

		DefaultBufferSize() : base(1024 * 16) {}
	}

	public interface IAdvance : ISelect<Composition, uint>, IResult<uint> {}

	public interface IToken : IResult<byte> {}

	public class Token : Instance<byte>, IToken
	{
		public Token(char instance) : base((byte)instance) {}
	}

	public class Content : Instance<uint>, IAdvance
	{
		readonly byte[] _content;

		public Content(string content) : this(Encoder.Default.Get(content)) {}

		public Content(byte[] content) : base((uint)content.Length) => _content = content;

		public uint Get(Composition parameter)
		{
			var result = (uint)_content.Length;
			_content.CopyInto(parameter.Output, 0, result, parameter.Index);
			return result;
		}
	}

	class ElementWriter<T> : IEmit<T>
	{
		readonly IEmit    _start, _finish;
		readonly IEmit<T> _content;

		public ElementWriter(IEmit start, IEmit<T> content, IEmit finish)
		{
			_start   = start;
			_content = content;
			_finish  = finish;
		}

		public Composition Get(Composition<T> parameter)
		{
			var content = _content.Get(_start.Get(parameter).Introduce(parameter.Instance));
			var result  = _finish.Get(content);
			return result;
		}
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

		public Array<byte> Get(T parameter) => _emitter.Get(new Composition<T>(_pool.Rent((int)_size), parameter))
		                                               .Complete(_pool);
	}

	public readonly struct Input<T>
	{
		public Input(Stream stream, T instance, CancellationToken cancel = new CancellationToken())
		{
			Stream   = stream;
			Instance = instance;
			Cancel   = cancel;
		}

		public Stream Stream { get; }

		public T Instance { get; }

		public CancellationToken Cancel { get; }
	}

	public readonly struct Lease<T> : IDisposable
	{
		readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

		readonly T[] _store;

		public Lease(T[] store, uint length)
		{
			_store = store;
			Length = length;
		}

		public ref T this[uint index] => ref _store[index];

		public uint Length { get; }

		public void Dispose()
		{
			Pool.Return(_store);
		}
	}

	/*public readonly struct Compositions
	{
		public Compositions(IList<Task> tasks, byte[] output, uint length)
		{
			Tasks  = tasks;
			Output = output;
			Length = length;
		}

		public IList<Task> Tasks { get; }

		public byte[] Output { get; }

		public uint Length { get; }
	}*/

	public interface ICompositor<T> : ISelect<T, Composition<T>>, ICommand<Composition> {}

	sealed class Compositor<T> : ICompositor<T>
	{
		public static Compositor<T> Default { get; } = new Compositor<T>();

		Compositor() : this(ArrayPool<byte>.Shared, DefaultBufferSize.Default) {}

		readonly ArrayPool<byte> _pool;
		readonly uint            _size;

		public Compositor(ArrayPool<byte> pool, uint size)
		{
			_pool = pool;
			_size = size;
		}

		public Composition<T> Get(T parameter) => new Composition<T>(_pool.Rent((int)_size), parameter);

		public void Execute(Composition parameter)
		{
			_pool.Return(parameter.Output);
			parameter.Output.Clear(parameter.Index);
		}
	}

	public interface IStagedWriter<T> : ISelect<Input<T>, Task> {}

	public sealed class SingleStagedWriter<T> : IStagedWriter<T>
	{
		readonly IEmit<T>       _instruction;
		readonly ICompositor<T> _compositor;

		public SingleStagedWriter(IEmit<T> instruction) : this(instruction, Compositor<T>.Default) {}

		public SingleStagedWriter(IEmit<T> instruction, ICompositor<T> compositor)
		{
			_instruction = instruction;
			_compositor  = compositor;
		}

		public async Task Get(Input<T> parameter)
		{
			var composition = _instruction.Get(_compositor.Get(parameter.Instance));
			var operation = parameter.Stream.WriteAsync(composition.Output, 0, (int)composition.Index,
			                                            parameter.Cancel);

			if (!operation.IsCompleted)
			{
				await operation;
			}

			_compositor.Execute(composition);
		}
	}

	public sealed class StagedWriter<T> : IStagedWriter<T>
	{
		readonly IInstructions<T> _instructions;
		readonly ICompositor<T>   _compositor;

		public StagedWriter(IInstructions<T> instructions) : this(instructions, Compositor<T>.Default) {}

		public StagedWriter(IInstructions<T> instructions, ICompositor<T> compositor)
		{
			_instructions = instructions;
			_compositor   = compositor;
		}

		public async Task Get(Input<T> parameter)
		{
			var composition = _compositor.Get(parameter.Instance);
			//var tasks       = new List<Task>();

			using (var instructions = _instructions.Get(composition))
			{
				//var page = ArrayPool<byte>.Shared.Rent((int)DefaultBufferSize.Default.Get());

				var length = instructions.Length;
				var last   = length - 1;
				for (uint i = 0u, start = 0u; i < length; i++)
				{
					var step = instructions[i].Get(composition);

					var complete = i == last;
					if (complete || step.Index >= step.Output.Length * .9)
					{
						var task = parameter.Stream.WriteAsync(step.Output, (int)start, (int)step.Index,
						                                       parameter.Cancel);
						if (!task.IsCompleted)
						{
							//tasks.Add(task.AsTask());
							await task;
						}

						step = new Composition(step.Output);

						if (complete)
						{
							_compositor.Execute(step);
							return;
						}

						start += step.Index;
					}

					composition = step.Introduce(parameter.Instance);
				}
			}

			/*var count = tasks.Count;
			for (var i = 0; i < count; i++)
			{
				var task = tasks[i];
				if (!task.IsCompleted)
				{
					await task;
				}
			}*/
		}
	}

	public delegate uint Advance<T>(Composition<T> parameter);

	public delegate uint Advance(Composition parameter);

	class Emit<T> : IEmit<T>
	{
		readonly uint       _size;
		readonly Advance<T> _advance;

		public Emit(uint size, Advance<T> advance)
		{
			_size    = size;
			_advance = advance;
		}

		public Composition Get(Composition<T> parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition<T>(parameter.Output.Copy(in _size), parameter.Instance,
				                                       parameter.Index)
				                  : parameter;
			return new Composition(composition.Output, parameter.Index + _advance(composition));
		}
	}

	class Emit : IEmit
	{
		readonly uint    _size;
		readonly Advance _advance;

		public Emit(IAdvance advance) : this(advance.Get(), advance.Get) {}

		public Emit(uint size, Advance advance)
		{
			_size    = size;
			_advance = advance;
		}

		public Composition Get(Composition parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition(parameter.Output.Copy(in _size), parameter.Index)
				                  : parameter;
			return new Composition(composition.Output, parameter.Index + _advance(composition));
		}
	}

	public interface IEmit : IAlteration<Composition> {}

	public interface IEmit<T> : ISelect<Composition<T>, Composition> {}

	sealed class EmptyEmit : IEmit
	{
		public static EmptyEmit Default { get; } = new EmptyEmit();

		EmptyEmit() {}

		public Composition Get(Composition parameter) => parameter;
	}

	// public interface IInstruction<T> : ISelect<Composition<T>>

	public interface IInstructions<T> : ISelect<Composition<T>, Lease<IEmit<T>>> {}

	/*class Instructions : IInstructions<uint>
	{
		public static Instructions Default { get; } = new Instructions();

		Instructions() : this(PositiveNumber.Default.Yield<IEmit<uint>>().Result()) {}

		readonly Array<IEmit<uint>> _instructions;

		public Instructions(Array<IEmit<uint>> instructions) => _instructions = instructions;

		public Lease<IEmit<uint>> Get(Composition<uint> parameter) => new Lease<IEmit<uint>>(_instructions, 1);
	}*/

	sealed class EmptyEmit<T> : IEmit<T>
	{
		public static EmptyEmit<T> Default { get; } = new EmptyEmit<T>();

		EmptyEmit() {}

		public Composition Get(Composition<T> parameter) => new Composition(parameter.Output, parameter.Index);
	}

	sealed class PositiveNumber : Emit<uint>
	{
		public static PositiveNumber Default { get; } = new PositiveNumber();

		PositiveNumber()
			: base(20, x => Utf8Formatter.TryFormat(x.Instance, x.Output.AsSpan((int)x.Index), out var count)
				                ? (uint)count
				                : throw new
					                  InvalidOperationException($"Could not format '{x.Instance}' into its UTF8 equivalent.")) {}
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