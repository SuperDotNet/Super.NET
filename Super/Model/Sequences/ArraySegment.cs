using Super.Model.Commands;
using Super.Model.Selection;
using Super.Runtime;
using System;

namespace Super.Model.Sequences
{
	public interface IIterator<T> : ISelect<IIteration<T>, T[]> {}

	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Store<T> store, ICommand<T[]> command)
		{
			Store = store;
			_command = command;
		}

		public Store<T> Store { get; }

		public void Dispose()
		{
			_command?.Execute(Store.Instance);
		}
	}

	

	public readonly struct ArrayView<T>
	{
		public static ArrayView<T> Empty { get; } = new ArrayView<T>(Empty<T>.Array);

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
}