using Super.Model.Commands;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public static class In<T>
	{
		public static ISpecification<T> Is(Func<T, bool> specification) => new DelegatedSpecification<T>(specification);

		public static ICommand<T> Then(Action<T> action) => new DelegatedCommand<T>(action);

		public static ISelect<T, TResult> Default<TResult>() => Select(Sources.Default<TResult>.Instance);

		public static ISelect<T, TResult> New<TResult>() => Select(Activator<TResult>.Default);

		public static ISelect<T, TResult> Out<TResult>() where TResult : IActivateMarker<T> => MarkedActivations<T, TResult>.Default;

		public static ISelect<T, TResult> Result<TResult>(TResult @this) => @this.ToSelect(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(ISource<TResult> @this) => @this.Allow(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(ISelect<TResult> select) => Select(select.Out<T>());

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);
	}

	public static class Select
	{
		public static ISelect<TParameter, TResult> New<TParameter, TResult>(I<TResult> _ = null)
			where TResult : IActivateMarker<TParameter> => MarkedActivations<TParameter, TResult>.Default;

		public static TResult To<T, TResult>(this T @this, ISelect<T, TResult> select)
			=> @this.To(select.ToDelegate());

		public static TResult To<T, TResult>(this T @this, Func<T, TResult> select) => select(@this);
	}
}