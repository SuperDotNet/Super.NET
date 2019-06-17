using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Serialization {
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

			@this.Clear((uint)@this.Length);

			Return(@this, false);

			return result;
		}

		public static IInstruction<T> Quoted<T>(this IInstruction<T> @this) => @this.Quoted(DoubleQuote.Default);

		public static IInstruction<T> Quoted<T>(this IInstruction<T> @this, IToken quote)
			=> new QuotedInstruction<T>(@this, quote.Get());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Composition<T> Introduce<T>(in this Composition @this, in T instance)
			=> new Composition<T>(@this.Output, in instance, @this.Index);
	}
}