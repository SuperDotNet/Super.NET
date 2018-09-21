using Super.Runtime;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public static class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Copy<T>(in this ArrayView<T> @this, T[] into, uint start = 0)
		{
			System.Array.ConstrainedCopy(@this.Array, (int)@this.Start, into, (int)start, (int)@this.Length);
			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Start || length != @this.Length ? new ArrayView<T>(@this.Array, index, length) : @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, in Selection selection)
		{
			var length = selection.Length.GetValueOrDefault(@this.Length);
			return selection.Start != @this.Start || length != @this.Length ?
				       new ArrayView<T>(@this.Array, selection.Start, length) : @this;
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<T> Get<T>(in this ArrayView<T> @this) => new Array<T>(@this.Start == 0 && @this.Length == @this.Array.Length ? @this.Array : @this.Copy());*/

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Source<T>(in this ArrayView<T> @this) => @this.Offset == 0 && @this.Count == @this.Array.Length ? @this.Array : @this.Copy();*/

		public static Array<T> Result<T>(in this ArrayView<T> @this) => new Array<T>(Collections.Result<T>.Default.Get(@this));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Copy<T>(in this ArrayView<T> @this)
		{
			var result = new T[@this.Length];
			@this.Copy(result);
			return result;
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