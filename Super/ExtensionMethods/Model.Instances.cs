using Super.Model.Containers;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.ExtensionMethods
{
	public static class Instances
	{
		public static IInstance<TTo> Select<TFrom, TTo>(this IInstance<TFrom> @this, ISelect<TTo> select)
			=> @this.Select(select.Out<TFrom>());

		public static IInstance<TTo> Select<TFrom, TTo>(this IInstance<TFrom> @this, ISource<TFrom, TTo> select)
			=> @this.Select(select.ToDelegate());

		public static IInstance<TTo> Select<TFrom, TTo>(this IInstance<TFrom> @this, Func<TFrom, TTo> select)
			=> new DelegatedParameterSource<TFrom, TTo>(select, @this.ToDelegate());

		public static IInstance<TResult> Select<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                             TParameter parameter)
			=> new FixedParameterSource<TParameter, TResult>(@this, parameter);

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(this IInstance<TResult> @this, ISource<TParameter, TResult> source)
			=> @this.Allow(I<TParameter>.Default).Or(source);

		public static IInstance<T> Or<T>(this IInstance<T> @this, IInstance<T> fallback)
			=> @this.Or(IsAssigned<T>.Default, fallback);

		public static IInstance<T> Or<T>(this IInstance<T> @this, ISpecification<T> specification, IInstance<T> fallback)
			=> new ValidatedInstance<T>(specification, @this, fallback);

		/*public static Func<ISource<TParameter, TResult>, TResult> Reduce<TParameter, TResult>(this IInstance<TParameter> @this, I<TResult> _) where TResult : IActivateMarker<TParameter>
			=> @this.Reduce;*/

		public static IInstance<TResult> Select<TParameter, TResult>(this IInstance<TParameter> @this, I<TResult> _)
			where TResult : IActivateMarker<TParameter>
			=> @this.Select(Activations<TParameter, TResult>.Default);

		public static TTo Get<TFrom, TTo>(this IInstance<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Select(select).Get();

		public static TResult Get<TParameter, TResult>(this IInstance<TParameter> @this,
		                                                  ISource<TParameter, TResult> source)
			=> @this.Get(source.ToDelegate());

		public static TResult Any<TParameter, TResult>(this IInstance<TResult> @this, TParameter _) => @this.Get();

		public static ISource<Unit, T> Allow<T>(this IInstance<T> @this) => @this.Allow(I<Unit>.Default);

		public static ISource<TParameter, TResult> Allow<TParameter, TResult>(this IInstance<TResult> @this, I<TParameter> _)
			=> new FixedDelegatedSource<TParameter, TResult>(@this.ToDelegate());

		public static IInstance<T> Singleton<T>(this IInstance<T> @this) => SingletonCoercer<T>.Default.Get(@this);

		public static IInstance<T> ToInstance<T>(this T @this) => Instances<T>.Default.Get(@this);

		public static Func<T> ToDelegate<T>(this IInstance<T> @this) => Super.Model.Instances.Delegates<T>.Default.Get(@this);

		/*public static IInstance<T> ToInstance<T>(this ISource<Unit, T> @this) => @this.Fix(Unit.Default);*/

		/*public static IInstance<TResult> Fix<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                          I<TResult> _)
			=> new DelegatedParameterSource<TParameter, TResult>(@this);

		public static IInstance<TResult> Fix<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                          Func<TParameter> parameter)
			=> new DelegatedParameterSource<TParameter, TResult>(@this.ToDelegate(), parameter);*/

		/*public static T Get<T>(this ISource<Unit, T> @this) => @this.Get(Unit.Default);*/

		/*public static ISource<Unit, T> Adapt<T>(this IInstance<T> @this) => Adapters<T>.Default.Get(@this);

		public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, ISource<TFrom, TTo> coercer)
			=> @this.Adapt(coercer.ToDelegate());

		public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, Func<TFrom, TTo> coercer)
			=> @this.Adapt().Out(coercer).ToInstance();*/

		/*public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, Cast<TTo> cast)
			=> @this.Adapt().Out(cast).ToInstance();

		public static IInstance<TTo> Adapt<TFrom, TTo>(this IInstance<TFrom> @this, I<TTo> activate) where TTo : IActivateMarker<TFrom>
			=> @this.Adapt().Out(activate).ToInstance();

		public static ISource<TParameter, ISource<Unit, T>> Adapt<TParameter, T>(this ISource<TParameter, IInstance<T>> @this)
			=> @this.Out(SourceAdapterCoercer<T>.Default);

		public static IInstance<Func<TParameter, TResult>> Delegate<TParameter, TResult>(this IInstance<ISource<TParameter, TResult>> @this)
			=> @this.Adapt(DelegateCoercer<TParameter, TResult>.Default);

		public static ISource<TParameter, TResult> Source<TParameter, TResult>(this IInstance<ISource<TParameter, TResult>> @this)
			=> new DelegatedInstanceSource<TParameter, TResult>(@this);*/

		/*public static ISource<TParameter, TResult> Adapt<TParameter, TResult>(this IInstance<ISource<TParameter, TResult>> @this, IInstance<ICommand<TResult>> command)
			=> @this.Adapt(command.Get());

		public static ISource<TParameter, TResult> Adapt<TParameter, TResult>(this IInstance<ISource<TParameter, TResult>> @this, ICommand<TResult> command)
			=> @this.To(InstanceValueCoercer<ISource<TParameter, TResult>>.Default)
			        .SelectOut(command.To(I<ConfiguringAlteration<TResult>>.Default));*/
	}
}