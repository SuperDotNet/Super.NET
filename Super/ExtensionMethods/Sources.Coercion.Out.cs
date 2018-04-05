using Super.Model.Commands;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Model.Sources.Coercion;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static ISource<TParameter, TTo> Account<TParameter, TTo>(this ISource<TParameter, object> @this,
		                                                                Cast<TTo> _)
			=> @this.Out(_)
			        .Or(@this.Out(Cast<IInstance<TTo>>.Default)
			                 .Assigned(InstanceValueCoercer<TTo>.Default));

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                     Cast<TTo> _)
			=> @this.Out(_, Default<TResult, TTo>.Instance);

		public static ISource<TParameter, TTo> Out<TParameter, TResult, TTo>(
			this ISource<TParameter, TResult> @this, Cast<TTo> _, ISource<TResult, TTo> fallback)
			=> @this.Out(CanCast<TResult, TTo>.Default.If(CastCoercer<TResult, TTo>.Default, fallback));

		public static ISource<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISource<TParameter, TFrom> @this, I<TTo> _)
			where TTo : IActivateMarker<TFrom> => @this.Out(Activations<TFrom, TTo>.Default);

		public static ISource<TParameter, TResult> Invoke<TParameter, TResult>(
			this ISource<TParameter, Func<TResult>> @this) => @this.Out(InvokeCoercer<TResult>.Default);

		public static ISource<TParameter, TTo> Assigned<TParameter, TResult, TTo>(this ISource<TParameter, TResult> @this,
		                                                                          ISource<TResult, TTo> coercer)
			=> @this.Out(coercer.In(AssignedSpecification<TResult>.Default));

		/*public static ISource<TParameter, TInstance> OutAs<TParameter, TResult, TInstance>(
			this ISource<TParameter, TResult> @this, I<TInstance> _)
			where TInstance : IActivateMarker<TResult> => @this.Out(Instances<TResult, TInstance>.Default);*/

		public static ISource<TParameter, TResult> Reduce<TParameter, TInput, TResult>(
			this ISource<TParameter, ISource<TInput, TResult>> @this)
			=> @this.Out(DelegatedParameterCoercer<TInput, TResult>.Default);

		public static ISource<TParameter, TResult> Reduce<TParameter, TInput, TResult>(
			this ISource<TParameter, ISource<TInput, TResult>> @this, IInstance<TInput> seed)
			=> Reduce(@this, seed.ToDelegate());

		public static ISource<TParameter, TResult> Reduce<TParameter, TInput, TResult>(
			this ISource<TParameter, ISource<TInput, TResult>> @this, Func<TInput> seed)
			=> @this.Out(new DelegatedParameterCoercer<TInput, TResult>(seed));

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
			=> @this.Guard(new Message<TParameter>(message));

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Guard(new GuardedFallback<TParameter, TResult>(message));

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> fallback)
			=> @this.Out(AssignedSpecification<TResult>.Default, fallback);

		public static ISource<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISource<TParameter, TResult> @this, I<TException> _) where TException : Exception
			=> Try(@this, _, Defaults<TParameter, TResult>.Default.Get(@this));

		public static ISource<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISource<TParameter, TResult> @this, I<TException> _, ISource<TParameter, TResult> fallback)
			where TException : Exception
			=> new Try<TException, TParameter, TResult>(@this.ToDelegate(), fallback.ToDelegate());

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

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new ConfiguringSource<TParameter, TResult>(@this, assignable);

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                    IInstance<ICommand<TResult>> command)
			=> @this.Out(new DelegatedInstanceCommand<TResult>(command));

		public static ISource<TParameter, TResult> Out<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                    ICommand<TResult> command)
			=> @this.Out(new ConfiguringAlteration<TResult>(command));

		public static ISource<TParameter, TResult> Service<TParameter, TResult>(
			this ISource<TParameter, IServiceProvider> @this, I<TResult> _)
			=> @this.Out(ServiceCoercer<TResult>.Default);


	}

	sealed class ServiceCoercer<T> : ISource<IServiceProvider, T>
	{
		public static ServiceCoercer<T> Default { get; } = new ServiceCoercer<T>();

		ServiceCoercer() {}

		public T Get(IServiceProvider parameter) => parameter.Get<T>();
	}
}