using Super.Model.Commands;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sequences
{
	public readonly struct Store<T> : IActivateUsing<T[]>
	{
		public static implicit operator Store<T>(T[] instance) => new Store<T>(instance);

		/*public static implicit operator T[](Store<T> instance) => instance.Instance;*/

		public Store(T[] instance) : this(instance, Assigned<uint>.Unassigned) {}

		public Store(T[] instance, in Assigned<uint> length)
		{
			Instance = instance;
			Length   = length;
		}

		public T[] Instance { get; }

		public Assigned<uint> Length { get; }
	}

	public readonly struct ArrayView<T>
	{
		public static ArrayView<T> Empty { get; } = new ArrayView<T>(Empty<T>.Array);

		public static implicit operator ArrayView<T>(T[] instance) => new ArrayView<T>(instance);

		public ArrayView(T[] array) : this(array, 0, (uint)array.Length) {}

		public ArrayView(T[] array, uint start, uint length)
		{
			Array  = array;
			Start  = start;
			Length = length;
		}

		public T[] Array { get; }

		public uint Start { get; }

		public uint Length { get; }
	}

	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Store<T> store, ICommand<T[]> command)
		{
			Store    = store;
			_command = command;
		}

		public Store<T> Store { get; }

		public void Dispose()
		{
			_command?.Execute(Store.Instance);
		}
	}
}