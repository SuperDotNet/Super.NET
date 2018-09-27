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

		public Session(T[] store, ICommand<T[]> command)
		{
			Array    = store;
			_command = command;
		}

		public T[] Array { get; }

		public void Dispose()
		{
			_command.Execute(Array);
		}
	}

	// ReSharper disable LocalSuppression

	public static class Extensions
	{
		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		public static Session<T> Session<T>(this IStores<T> @this, in Store<T> store) => new Session<T>(store.Instance, @this);

		public static T[] Into<T>(in this ArrayView<T> @this, T[] into)
			=> @this.Array.Copy(into, new Selection(@this.Start, @this.Length));

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Copy<T>(this T[] @this, T[] result, Selection? selection = null, uint offset = 0)
		{
			Array.Copy(@this, selection?.Start ?? 0,
			           result, offset, selection?.Length ?? (uint)Math.Min(result.Length, @this.Length));
			return result;
		}

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Start || length != @this.Length ? new ArrayView<T>(@this.Array, index, length) : @this;
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