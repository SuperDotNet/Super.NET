using System;
using System.Reactive;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static TResult Allow<TParameter, TResult>(this IInstance<TResult> @this, TParameter _) => @this.Get();

		public static IInstance<T> Singleton<T>(this IInstance<T> @this) => SingletonCoercer<T>.Default.Get(@this);

		public static T Get<T>(this ISource<Unit, T> @this) => @this.Get(Unit.Default);

		public static ISource<Unit, T> Adapt<T>(this IInstance<T> @this) => Adapters<T>.Default.Get(@this);

		public static ISource<TParameter, ISource<Unit, T>> Adapt<TParameter, T>(this ISource<TParameter, IInstance<T>> @this)
			=> @this.Out(SourceAdapterCoercer<T>.Default);

		public static IInstance<T> ToResult<T>(this ISource<Unit, T> @this) => @this.Fix(Unit.Default);

		public static IInstance<TResult> Fix<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                          TParameter parameter)
			=> new FixedParameterSource<TParameter, TResult>(@this, parameter);

		public static Func<T> ToDelegate<T>(this IInstance<T> @this) => @this.Adapt().Get;
	}
}