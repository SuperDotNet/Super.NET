using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public static class In<T>
	{
		public static ISelect<T, TResult> Out<TResult>() => Default<T, TResult>.Instance;

		public static ISelect<T, TResult> New<TResult>(TResult @this) => @this.ToSource(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(ISelect<TResult> select) => Select(select.Out<T>());

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);
	}

	public static class Select
	{
		public static ISelect<TParameter, TResult> New<TParameter, TResult>(I<TResult> _ = null)
			where TResult : IActivateMarker<TParameter>
			=> Activations<TParameter, TResult>.Default;

		public static ISelect<TParameter, TResult> From<TParameter, TResult>(Func<TParameter, TResult> source)
			=> In<TParameter>.Select(source);
	}
}