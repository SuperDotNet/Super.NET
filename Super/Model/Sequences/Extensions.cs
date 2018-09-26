using Super.Model.Collections;
using Super.Model.Selection;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	static class Extensions
	{
		public static Query<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new ArrayQuery<_, T>(@this);

		public static Query<_, T> Select<_, T>(this Query<_, T> @this, Collections.Selection selection)
			=> @this.Skip(selection.Start).Take(selection.Length.GetValueOrDefault());

		public static Query<_, T> Skip<_, T>(this Query<_, T> @this, uint skip)
			=> new Query<_, T>(@this.Select,
			                   new Definition<T>(@this.Definition.Segment,
			                                     new Collections.Selection(@this.Definition.Selection.Start + skip,
			                                                               @this.Definition.Selection.Length - skip)));

		public static Query<_, T> Take<_, T>(this Query<_, T> @this, uint take)
			=> new Query<_, T>(@this.Select,
			                   new Definition<T>(@this.Definition.Segment,
			                                     new Collections.Selection(@this.Definition.Selection.Start, take)));

		public static ISelect<_, Array<T>> Result<_, T>(this Query<_, T> @this) => @this.Get().Array();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] New<T>(this IStores<T> @this, T[] reference)
			=> @this.New(reference, 0, (uint)reference.Length);

		/*public static T[] New<T>(this IStores<T> @this, in Store<T> store)
			=> @this.New(store.Instance, 0, store.Length);*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] New<T>(this IStores<T> @this, T[] reference, in Collections.Selection selection)
			=> @this.New(reference, selection.Start, selection.Length ?? (uint)reference.Length);

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] New<T>(this IStores<T> @this, T[] reference, uint start, uint length)
		{
			var result = @this.Get(length);
			System.Array.Copy(reference, start, result, 0, length);
			return result;
		}

		/*public static Store<T> Store<T>(this IStores<T> @this, uint length) => new Store<T>(@this.Get(length), length);*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] New<T>(this T[] @this) => Allocated<T>.Default.New(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Session<T> Session<T>(this IStores<T> @this, T[] items) => new Session<T>(items, @this);
	}
}