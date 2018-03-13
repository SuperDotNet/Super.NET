using System;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Model.Sources.Coercion;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(
			this ISource<TParameter, TResult> @this, I<TTo> _)
			=> @this.Out(AssignedSpecification<TResult>.Default.If(CastCoercer<TResult, TTo>.Default,
			                                                       Default<TResult, TTo>.Instance));

		public static ISource<TParameter, TResult> Invoke<TParameter, TResult>(
			this ISource<TParameter, Func<TResult>> @this) => @this.Out(DelegateCoercer<TResult>.Default);

		public static ISource<TParameter, TTo> Assigned<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                          ISource<TResult, TTo> coercer)
			=> @this.Out(coercer.In(AssignedSpecification<TResult>.Default));

		public static ISource<TParameter, TInstance> OutAs<TParameter, TResult, TInstance>(
			this ISource<TParameter, TResult> @this,
			I<TInstance> _)
			where TInstance : IActivateMarker<TResult> => @this.Out(Instances<TResult, TInstance>.Default);

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     ISource<TResult, TTo> coercer)
			=> @this.Out(coercer.ToDelegate());

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     Func<TResult, TTo> coercer)
			=> @this.ToDelegate().Out(coercer);

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this Func<TParameter, TResult> @this,
		                                                                     Func<TResult, TTo> coercer)
			=> new CoercedResult<TParameter, TResult, TTo>(@this, coercer);

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                    IAlteration<TResult> alteration)
			=> new AlteredSource<TParameter, TResult>(@this.ToDelegate(), alteration.ToDelegate());

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this) => Guard(@this, GuardedFallback<TParameter, TResult>.Default);

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, Func<TParameter, string> message)
			=> Guard(@this, new Message<TParameter, TResult>(message));

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IMessage<TParameter> message)
			=> Guard(@this, new GuardedFallback<TParameter, TResult>(message));

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> fallback)
			=> @this.Out(AssignedSpecification<TResult>.Default, fallback);

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> @this.Out(specification, Defaults<TParameter, TResult>.Default.Get(@this));

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> fallback)
			=> Out(@this, AssignedSpecification<TResult>.Default, fallback);

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISource<TParameter, TResult> fallback)
			=> new ValidatedResult<TParameter, TResult>(specification, @this, fallback);
	}
}