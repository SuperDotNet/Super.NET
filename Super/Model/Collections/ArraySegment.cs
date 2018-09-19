using Super.Runtime;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public static class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Copy<T>(in this ArrayView<T> @this, T[] into, uint start = 0)
		{
			System.Array.ConstrainedCopy(@this.Array, (int)@this.Offset, into, (int)start, (int)@this.Count);
			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Offset || length != @this.Count ? new ArrayView<T>(@this.Array, index, length) : @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<T> Get<T>(in this ArrayView<T> @this) => new Array<T>(@this.Offset == 0 && @this.Count == @this.Array.Length ? @this.Array : @this.Copy());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Copy<T>(in this ArrayView<T> @this)
		{
			var result = new T[@this.Count];
			@this.Copy(result);
			return result;
		}
	}

	public readonly struct ArrayView<T>
	{
		public static ArrayView<T> Empty { get; } = new ArrayView<T>(Empty<T>.Array);

		public ArrayView(T[] array) : this(array, 0, (uint)array.Length) {}

		public ArrayView(T[] array, uint offset, uint count)
		{
			Array  = array;
			Offset  = offset;
			Count = count;
		}

		public T[] Array { get; }

		public uint Offset { get; }

		public uint Count { get; }
	}
}