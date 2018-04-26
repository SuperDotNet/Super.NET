using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super
{
	public static class In<T>
	{
		public static ISpecification<T> Is(Func<T, bool> specification) => new DelegatedSpecification<T>(specification);

		public static ICommand<T> Then(Action<T> action) => new DelegatedCommand<T>(action);

		public static ISelect<T, TResult> Default<TResult>() => Select(Model.Sources.Default<TResult>.Instance);

		public static ISelect<T, TResult> Cast<TResult>() => Runtime.Objects.Cast<T, TResult>.Default;

		public static ISelect<T, TResult> CastForValue<TResult>() => Runtime.Objects.ValueAwareCast<T, TResult>.Default;

		public static ISelect<T, TResult> Cast<TFrom, TResult>(ISelect<TFrom, TResult> @this) => Cast(@this.ToDelegate());

		public static ISelect<T, TResult> Cast<TFrom, TResult>(Func<TFrom, TResult> @this) => Runtime.Objects.Cast<T, TFrom>.Default.Out(@this);

		public static ISelect<T, TResult> New<TResult>() => Select(Activator<TResult>.Default);

		public static ISelect<T, TResult> Out<TResult>() where TResult : IActivateMarker<T> => MarkedActivations<T, TResult>.Default;

		public static ISelect<T, TResult> Start<TResult>(TResult @this) => @this.Enter(I<T>.Default);

		public static ISelect<T, T> Start() => Self<T>.Default;

		public static ISelect<T, TResult> Start<TResult>(Func<ISelect<T, T>, ISelect<T, TResult>> select)
			=> Self<T>.Default.Out(select);

		public static ISelect<T, TResult> Select<TResult>(ISource<TResult> @this) => @this.Allow(I<T>.Default);

		public static ISelect<T, TResult> Select<TResult>(Func<T, TResult> @this)
			=> Selections<T, TResult>.Default.Get(@this);
	}
}