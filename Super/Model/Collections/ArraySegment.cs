using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IIterator<T> : ISelect<IIteration<T>, T[]> {}

	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Sequences.Store<T> store, ICommand<T[]> command)
		{
			Store = store;
			_command = command;
		}

		public Sequences.Store<T> Store { get; }

		public void Dispose()
		{
			_command?.Execute(Store.Instance);
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

		// ReSharper disable once TooManyArguments
		public static T[] CopyInto<T>(this T[] @this, T[] result, in Selection selection, uint offset = 0)
			=> @this.CopyInto(result, selection.Start, selection.Length.IsAssigned
				                                       ? selection.Length.Instance
				                                       : (uint)result.Length - offset, offset);

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result, uint start, uint length, uint offset = 0)
		{
			Array.Copy(@this, start,
			           result, offset, length);
			return result;
		}

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Sequences.Store<T> CopyInto<T>(this T[] @this, in Sequences.Store<T> store, uint start, uint offset = 0)
		{
			Array.Copy(@this, start, store.Instance, offset, store.Length - offset);
			return store;
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