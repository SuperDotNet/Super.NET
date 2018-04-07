using Super.Model.Collections;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISource<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(this ISource<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManyCoercer<TFrom, TTo>>.Default);

		public static ISource<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISource<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectCoercer<TFrom, TTo>>.Default);

		/*public static IContainer<T> Enclose<T>(this IInstance<T> @this) => Activations<IInstance<T>, Container<T>>.Default.Get(@this);

		public static IContainer<TTo> To<TFrom, TTo>(this IContainer<TFrom> @this, ISelector<TTo> selector) => @this.Select(selector.For<TFrom>());*/

		/*public static ISource<ImmutableArray<TParameter>, TResult> Enumerate<TParameter, TResult>(
			this ISource<IEnumerable<TParameter>, TResult> @this)
			=> @this.In(EnumerableSelector<TParameter>.Default);

		public static ISource<TParameter, ImmutableArray<TResult>> Enumerate<TParameter, TResult>(
			this ISource<TParameter, IEnumerable<TResult>> @this)
			=> @this.Out(ImmutableArraySelector<TResult>.Default);

		public static ISource<IEnumerable<TParameter>, TResult> AsEnumerable<TParameter, TResult>(
			this ISource<ImmutableArray<TParameter>, TResult> @this)
			=> @this.In(ImmutableArraySelector<TParameter>.Default);

		public static ISource<TParameter, IEnumerable<TResult>> AsEnumerable<TParameter, TResult>(
			this ISource<TParameter, ImmutableArray<TResult>> @this)
			=> @this.Out(EnumerableSelector<TResult>.Default);*/

		/*public static ISource<ImmutableArray<TTo>, TResult> In<TFrom, TTo, TResult>(
			this ISource<ImmutableArray<TFrom>, TResult> @this, ISource<TTo, TFrom> coercer)
			=> @this.AsEnumerable().In(coercer.ToDelegate()).Enumerate();

		public static ISource<IEnumerable<TTo>, TResult> In<TFrom, TTo, TResult>(
			this ISource<IEnumerable<TFrom>, TResult> @this, ISource<TTo, TFrom> coercer)
			=> @this.In(coercer.ToDelegate());

		public static ISource<IEnumerable<TFrom>, TResult> In<TResult, TFrom, TTo>(
			this ISource<IEnumerable<TTo>, TResult> @this, Func<TFrom, TTo> coercer)
			=> @this.In(new SelectCoercer<TFrom, TTo>(coercer));*/

		/*public static ISource<TParameter, IEnumerable<TTo>> Out<TParameter, TFrom, TTo>(
			this ISource<TParameter, IEnumerable<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.SelectOut(new SelectCoercer<TFrom, TTo>(select));*/
	}
}