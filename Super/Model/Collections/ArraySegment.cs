using Super.Runtime;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	// ReSharper disable LocalSuppression

	public static class Extensions
	{
		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> Copy<T>(in this Store<T> @this, T[] into, uint start = 0)
		{
			System.Array.Copy(@this.Instance, 0, into, (int)start, (int)@this.Length);
			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Copied<T>(in this ArrayView<T> @this, T[] into, uint start = 0)
		{
			System.Array.Copy(@this.Array, @this.Start, into, (int)start, (int)@this.Length);
			return into;
		}*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Into<T>(in this ArrayView<T> @this, T[] into, uint start = 0)
		{
			System.Array.Copy(@this.Array, @this.Start, into, (int)start, (int)@this.Length);
			return into;
		}



		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> Copy<T>(in this Store<T> @this, T[] into, uint start = 0)
		{
			System.Array.Copy(@this.Instance, 0, into, (int)start, (int)@this.Length);
			return @this;
		}*/

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Into<T>(in this Store<T> @this, T[] into, uint start = 0)
		{
			System.Array.Copy(@this.Instance, 0, into, (int)start, (int)@this.Length);
			return into;
		}*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> CopyInto<T>(in this Store<T> @this, in Store<T> into, Selection? selection = null)
		{
			var index = selection?.Start ?? 0;
			System.Array.Copy(@this.Instance, 0, into.Instance, index, selection?.Length ?? @this.Length - index);
			return into;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> Resize<T>(in this Store<T> @this, uint size) => new Store<T>(@this.Instance, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Start || length != @this.Length ? new ArrayView<T>(@this.Array, index, length) : @this;
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<T> Result<T>(in this ArrayView<T> @this) => new Array<T>(Collections.Result<T>.Default.Get(@this));*/
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