using Super.Model.Collections;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public static class Extensions
	{
		public static IQuery<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new ArrayQuery<_, T>(@this);

		/*public static IQuery<_, T> Alter<_, T>(this IQuery<_, T> @this, IAlterSelection<T> parameter)
			=> I<QueryAlteration<_, T>>.Default
			                           .From(parameter)
			                           .Get(@this);*/

		public static IQuery<_, T> Select<_, T>(this IQuery<_, T> @this, Collections.Selection selection)
			=> @this.Skip(selection.Start)
			        .Take(selection.Length);

		public static IQuery<_, T> Skip<_, T>(this IQuery<_, T> @this, uint skip) => @this.Get(new Skip<T>(skip));

		public static IQuery<_, T> Take<_, T>(this IQuery<_, T> @this, uint take) => @this.Get(new Take<T>(take));

		public static ISelect<_, Array<T>> Result<_, T>(this IQuery<_, T> @this) => @this.Get().Array();

		/**/

		public static Session<T> Session<T>(this IStore<T> @this, uint amount)
			=> new Session<T>(@this.Get(amount), @this, amount);

		/*public static Session<T> Session<T>(this IStore<T> @this, uint amount)
			=> @this.Session(new Store<T>(@this.Get(amount), amount));*/

		public static Session<T> Session<T>(this IStore<T> @this, T[] items) => new Session<T>(items, @this);

		public static Session<T> Session<T>(this IStore<T> @this, in Store<T> store)
			=> new Session<T>(store.Instance, @this, store.Length);
	}
}