using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;

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
			=> @this.Unless(IsTypeSpecification<TParameter, TOther>.Default, other.In(I<TParameter>.Default));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                       ISpecification<TParameter, TResult> source)
			=> @this.Unless(source, source);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISource<TParameter, TResult> @this, I<TOther> _) where TOther : IInstance<TResult>
			=> @this.Unless(IsTypeSpecification<TParameter, TOther>.Default,
			                SourceCoercer<TResult>.Default.In(I<TParameter>.Default));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TParameter> specification,
			TResult other)
			=> @this.Unless(specification, new Instance<TParameter, TResult>(other));

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TParameter> specification,
			ISource<TParameter, TResult> other)
			=> Unless(@this, specification, AlwaysSpecification<TResult>.Default, other);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult>(this ISource<TParameter, TResult> @this,
		                                                                       ISpecification<TParameter> specification,
		                                                                       ISpecification<TResult> result,
		                                                                       ISource<TParameter, TResult> other)
			=> new Conditional<TParameter, TResult>(specification, other.Out(result, @this), @this);
	}
}