using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Serialization
{
	public interface IStagedWriter<T> : ISelect<Input<T>, Task> {}

	public sealed class SingleStageWriter<T> : IStagedWriter<T>
	{
		readonly IInstruction<T> _instruction;
		readonly IStorage<byte>  _storage;

		public SingleStageWriter(IInstruction<T> instruction) : this(instruction, Leases<byte>.Default) {}

		public SingleStageWriter(IInstruction<T> instruction, IStorage<byte> storage)
		{
			_instruction = instruction;
			_storage     = storage;
		}

		public async Task Get(Input<T> parameter)
		{
			using (var session = _storage.Session(_instruction.Get(parameter.Instance)))
			{
				var operation = parameter.Stream
				                         .WriteAsync(session.Store,
				                                     0,
				                                     (int)_instruction.Get(new Composition<T>(session.Store,
				                                                                              parameter.Instance)),
				                                     parameter.Cancel);

				if (!operation.IsCompleted)
				{
					await operation;
				}
			}
		}
	}

	/*public sealed class StagedWriter<T> : IStagedWriter<T>
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

			using (var instructions = _instructions.Get(composition))
			{
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
		}
	}*/

	/*public interface ICompositor<T> : ISelect<T, Composition<T>>, ICommand<Composition> {}

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
			parameter.Output.Clear(parameter.Index);
			_pool.Return(parameter.Output);
		}
	}

	public interface IInstructions<T> : ISelect<Composition<T>, Lease<IEmit<T>>> {}*/

	/*class Instructions : IInstructions<uint>
	{
		public static Instructions Default { get; } = new Instructions();

		Instructions() : this(PositiveNumber.Default.Yield<IEmit<uint>>().Result()) {}

		readonly Array<IEmit<uint>> _instructions;

		public Instructions(Array<IEmit<uint>> instructions) => _instructions = instructions;

		public Lease<IEmit<uint>> Get(Composition<uint> parameter) => new Lease<IEmit<uint>>(_instructions, 1);
	}*/

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
}