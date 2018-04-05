using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
// ReSharper disable TooManyArguments

namespace Super.ExtensionMethods
{
	public static partial class Sources
	{
		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> next)
			=> @this.Out(AssignedSpecification<TResult>.Default, next);

		public static ISource<TParameter, TResult> Into<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISource<TParameter, TResult> @this, ISource<TOther, TResult> other)
			=> @this.Unless(IsTypeSpecification<TParameter, TOther>.Default, other.In(Cast<TParameter>.Default));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                       ISpecification<TParameter, TResult> source)
			=> @this.Unless(source, source);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISource<TParameter, TResult> @this, I<TOther> _) where TOther : IInstance<TResult>
			=> @this.Unless(IsTypeSpecification<TParameter, TOther>.Default,
			                InstanceValueCoercer<TResult>.Default.In(Cast<TParameter>.Default));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TParameter> specification,
			TResult other) => @this.Unless(specification, new Fixed<TParameter, TResult>(other));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TParameter> specification,
			ISource<TParameter, TResult> other)
			=> Unless(@this, specification, AlwaysSpecification<TResult>.Default, other);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                       ISpecification<TParameter> specification,
		                                                                       ISpecification<TResult> result,
		                                                                       ISource<TParameter, TResult> other)
			=> new Conditional<TParameter, TResult>(specification, other.Out(result, @this), @this);

		public static TTo To<TFrom, TTo>(this TFrom @this, ISource<TFrom, TTo> coercer) => coercer.Get(@this);

		public static ISource<TParameter, TResult> To<TFrom, T, TParameter, TResult>(
			this IInstance<TFrom> @this, Source<T, TParameter, TResult> _)
			where T : ISource<TParameter, TResult>, IActivateMarker<TFrom>
			=> @this.To(I<T>.Default).Adapt(Cast<ISource<TParameter, TResult>>.Default).Source();

		public static Func<TTo> ToDelegate<T, TTo>(this IInstance<T> @this, I<TTo> infer) where TTo : IActivateMarker<T>
			=> @this.To(infer).ToDelegate();

		public static IInstance<TTo> To<T, TTo>(this IInstance<T> _, I<TTo> infer) where TTo : IActivateMarker<T>
			=> Activations<T, TTo>.Default.Fix(infer);

		public static ISource<TFrom, TTo> To<TFrom, TTo>(this I<TFrom> __, I<TTo> _) where TTo : IActivateMarker<TFrom>
			=> Activations<TFrom, TTo>.Default;

		public static TTo To<T, TTo>(this T @this, I<TTo> _) where TTo : IActivateMarker<T>
			=> Activations<T, TTo>.Default.Get(@this);

		public static ISource<TFrom, TTo> New<TFrom, TTo>(this I<TFrom> __, I<TTo> _) => Instances<TFrom, TTo>.Default;

		public static TTo New<T, TTo>(this T @this, I<TTo> _) => Instances<T, TTo>.Default.Get(@this);
	}
}