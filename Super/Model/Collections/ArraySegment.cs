using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime;
using System;

namespace Super.Model.Collections
{
	public interface IIterator<T> : ISelect<IIteration<T>, T[]> {}

	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(T[] store, ICommand<T[]> command) : this(store, command, (uint)store.Length) {}

		public Session(T[] store, ICommand<T[]> command, uint length)
		{
			Array    = store;
			Length   = length;
			_command = command;
		}

		public T[] Array { get; }
		public uint Length { get; }

		public void Dispose()
		{
			_command?.Execute(Array);
		}
	}

	// ReSharper disable LocalSuppression

	public static class Extensions
	{
		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		public static Session<T> Session<T>(this IStores<T> @this, in Store<T> store)
			=> new Session<T>(store.Instance, @this);

		public static T[] Into<T>(in this ArrayView<T> @this, T[] into)
			=> @this.Array.CopyInto(into, @this.Start, @this.Length);

		public static T[] CopyInto<T>(this T[] @this, T[] result, uint offset = 0)
			=> @this.CopyInto(result, Selection.Default, offset);

		// ReSharper disable once TooManyArguments
		public static T[] CopyInto<T>(this T[] @this, T[] result, in Selection selection, uint offset = 0)
			=> @this.CopyInto(result, selection.Start, selection.Length.IsAssigned
				                                       ? selection.Length.Instance
				                                       : (uint)result.Length - offset, offset);

		// ReSharper disable once TooManyArguments
		public static T[] CopyInto<T>(this T[] @this, T[] result, uint start, uint length, uint offset = 0)
		{
			Array.Copy(@this, start,
			           result, offset, length);
			return result;
		}

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Start || length != @this.Length
				       ? new ArrayView<T>(@this.Array, index, length)
				       : @this;
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