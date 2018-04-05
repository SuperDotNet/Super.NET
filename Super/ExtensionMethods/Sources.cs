using System;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISource<TParameter, TResult> @this)
			=> Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToStore<TParameter, TResult>(this ISource<TParameter, TResult> @this)
			where TParameter : class => @this.ToDelegate().ToStore();

		public static ISpecification<TParameter, TResult> ToStore<TParameter, TResult>(
			this ISpecification<TParameter, TResult> @this)
			where TParameter : class
			=> new SpecificationSource<TParameter, TResult>(@this.Adapt().ToStore().ToSpecification(),
			                                                @this.ToDelegate().ToStore());

		public static ISource<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceStores<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> new ConditionalInstance<TParameter, TResult>(specification, @this, default);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> Sources<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult, TAttribute>(
			this TResult @this, I<TAttribute> _) where TAttribute : Attribute
			=> @this.ToSource(IsDefinedSpecification<TAttribute>
			                  .Default.Adapt()
			                  .In(InstanceMetadataCoercer<TParameter>.Default)
			                  .ToSpecification());
	}
}