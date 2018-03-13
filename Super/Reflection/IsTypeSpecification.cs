using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class IsTypeSpecification<TParameter, T> : ISpecification<TParameter>
	{
		public static IsTypeSpecification<TParameter, T> Default { get; } = new IsTypeSpecification<TParameter, T>();

		IsTypeSpecification() {}

		public bool IsSatisfiedBy(TParameter parameter) => parameter is T;
	}

	public sealed class IsTypeSpecification<T> : DecoratedSpecification<object>
	{
		public static IsTypeSpecification<T> Default { get; } = new IsTypeSpecification<T>();

		IsTypeSpecification() : base(IsTypeSpecification<object, T>.Default) {}
	}
}