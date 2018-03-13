using System;
using System.Reflection;
using Super.Model.Sources;
using Super.Model.Sources.Coercion;
using Super.Model.Sources.Tables;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static ITable<TTo, TResult> In<TFrom, TTo, TResult>(
			this ITable<TFrom, TResult> @this, ISource<TTo, TFrom> coercer)
			=> new CoercedTable<TFrom, TTo, TResult>(@this, coercer);

		public static ITable<TParameter, TResult> Guarded<TParameter, TResult>(this ITable<TParameter, TResult> @this)
			=> @this.In(AssignedArgumentGuard<TParameter>.Default);

		public static ITable<TParameter, TResult> Assigned<TParameter, TResult>(this ITable<TParameter, TResult> @this)
			=> @this.In(AssignedSpecification<TParameter>.Default);

		public static ITable<TParameter, TResult> In<TParameter, TResult>(
			this ITable<TParameter, TResult> @this, ISpecification<TParameter> specification)
			=> new ValidatedTable<TParameter, TResult>(specification, @this);

		public static ISpecification<TFrom, TResult> In<TFrom, TTo, TResult>(
			this ISpecification<TTo, TResult> @this, ISource<TFrom, TTo> coercer)
			=> new SpecificationSource<TFrom, TResult>(@this.Adapt().In(coercer).ToSpecification(),
			                                           @this.ToDelegate().In(coercer.ToDelegate()));

		public static ISource<Decoration<TFrom, TResult>, TResult> In<TFrom, TTo, TResult>(
			this ISource<Decoration<TTo, TResult>, TResult> @this, I<TFrom> _)
			=> @this.In(DecorationParameterCoercer<TFrom, TTo, TResult>.Default);

		public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                   ISpecification<TParameter> specification)
			=> @this.In(specification, Defaults<TParameter, TResult>.Default.Get(@this));

		public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                   ISpecification<TParameter> specification,
		                                                                   ISource<TParameter, TResult> @false)
			=> new ValidatedSource<TParameter, TResult>(specification, @this, @false);

		public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this, I<TFrom> _)
			=> In(@this, CastCoercer<TFrom, TTo>.Default);

		public static ISource<TFrom, TResult> Assigned<TFrom, TTo, TResult>(
			this ISource<TTo, TResult> @this, ISource<TFrom, TTo> coercer)
			=> coercer.Out(AssignedSpecification<TTo>.Default.If(@this, Defaults<TTo, TResult>.Default.Get(@this)));

		public static ISource<TTo, TResult> In<TFrom, TTo, TResult>(this ISource<TFrom, TResult> @this,
		                                                            ISource<TTo, TFrom> factory)
			=> @this.In(factory.ToDelegate());

		public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this,
		                                                              Func<TFrom, TTo> coercer)
			=> @this.ToDelegate().In(coercer);

		public static ISource<TTo, TResult> In<TFrom, TTo, TResult>(this Func<TFrom, TResult> @this,
		                                                            Func<TTo, TFrom> coercer)
			=> new CoercedParameter<TFrom, TTo, TResult>(@this, coercer);

		public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<Type, TResult> @this,
		                                                                   I<TParameter> _ = null)
			=> @this.In(InstanceTypeCoercer<TParameter>.Default);

		public static ISource<TParameter, TResult> In<TParameter, TResult>(this ISource<TypeInfo, TResult> @this,
		                                                                   I<TParameter> _ = null)
			=> @this.In(InstanceMetadataCoercer<TParameter>.Default);
	}
}