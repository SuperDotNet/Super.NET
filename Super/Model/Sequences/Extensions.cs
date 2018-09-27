using Super.Model.Collections;
using Super.Model.Selection;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	public static class Extensions
	{
		public static IQuery<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new ArrayQuery<_, T>(@this);

		public static IQuery<_, T> Alter<_, T>(this IQuery<_, T> @this, IAlterDefinition<T> parameter)
			=> new QueryAlteration<_, T>(parameter).Get(@this);

		public static IQuery<_, T> Select<_, T>(this IQuery<_, T> @this, Collections.Selection selection)
			=> @this.Skip(selection.Start).Take(selection.Length.GetValueOrDefault());

		public static IQuery<_, T> Skip<_, T>(this IQuery<_, T> @this, uint skip) => @this.Alter(new Skip<T>(skip));

		public static IQuery<_, T> Take<_, T>(this IQuery<_, T> @this, uint take)
			=> @this.Alter(new Take<T>(take));

		public static ISelect<_, Array<T>> Result<_, T>(this IQuery<_, T> @this) => @this.Get().Array();

		public static T[] New<T>(this IStores<T> @this, T[] reference)
			=> @this.New(reference, new Collections.Selection(0, (uint)reference.Length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] New<T>(this IStores<T> @this, T[] reference, in Collections.Selection selection)
			=> reference.Copy(@this.Get(selection.Length ?? (uint)reference.Length), selection);

		public static T[] New<T>(this T[] @this) => Allocated<T>.Default.New(@this);

		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		public static Session<T> Session<T>(this IStores<T> @this, T[] items) => new Session<T>(items, @this);
	}
}