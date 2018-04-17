using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public static class In<T>
	{
		/*public static Func<T, TResult> From<TResult>(Func<T, TResult> select) => select;*/

		public static ISelect<T, TResult> Out<TResult>() where TResult : IActivateMarker<T> => Activations<T, TResult>.Default;

		public static ISelect<T, TResult> New<TResult>(TResult @this) => @this.ToSelect(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(ISelect<TResult> select) => Select(select.Out<T>());

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);
	}

	public static class Select
	{
		public static ISelect<TParameter, TResult> New<TParameter, TResult>(I<TResult> _ = null)
			where TResult : IActivateMarker<TParameter>
			=> Activations<TParameter, TResult>.Default;

		public static TResult To<T, TResult>(this T @this, ISelect<T, TResult> select)
			=> @this.To(@select.ToDelegate());

		public static TResult To<T, TResult>(this T @this, Func<T, TResult> select) => @select(@this);
	}
}