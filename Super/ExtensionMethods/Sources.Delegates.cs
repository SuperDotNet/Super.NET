using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Reflection;
using System;
using System.Reactive;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static TResult Allow<TParameter, TResult>(this IInstance<TResult> @this, TParameter _) => @this.Get();

		public static ISource<object, T> Allow<T>(this IInstance<T> @this) => @this.Allow(I<object>.Default);

		public static ISource<TParameter, TResult> Allow<TParameter, TResult>(this IInstance<TResult> @this, I<TParameter> _)
			=> new FixedDelegatedSource<TParameter, TResult>(@this.ToDelegate());

		public static IInstance<T> Singleton<T>(this IInstance<T> @this) => SingletonCoercer<T>.Default.Get(@this);

		public static T Get<T>(this ISource<Unit, T> @this) => @this.Get(Unit.Default);

		public static ISource<Unit, T> Adapt<T>(this IInstance<T> @this) => Adapters<T>.Default.Get(@this);

		public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, ISource<TFrom, TTo> coercer)
			=> Adapt(@this, coercer.ToDelegate());

		public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, Func<TFrom, TTo> coercer)
			=> @this.Adapt().Out(coercer).ToInstance();

		public static ISource<TParameter, ISource<Unit, T>> Adapt<TParameter, T>(this ISource<TParameter, IInstance<T>> @this)
			=> @this.Out(SourceAdapterCoercer<T>.Default);

		public static IInstance<T> ToInstance<T>(this T @this) => new Instance<T>(@this);

		public static IInstance<T> ToInstance<T>(this ISource<Unit, T> @this) => @this.Fix(Unit.Default);

		public static IInstance<TResult> Fix<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                          TParameter parameter)
			=> new FixedParameterSource<TParameter, TResult>(@this, parameter);

		public static IInstance<TResult> Fix<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                          I<TResult> _)
			=> new DelegatedParameterSource<TParameter, TResult>(@this);

		public static Func<T> ToDelegate<T>(this IInstance<T> @this) => @this.Adapt().Get;
	}
}