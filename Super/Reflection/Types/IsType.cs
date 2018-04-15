using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class IsType<TParameter, T> : ISpecification<TParameter>
	{
		public static IsType<TParameter, T> Default { get; } = new IsType<TParameter, T>();

		IsType() {}

		public bool IsSatisfiedBy(TParameter parameter) => parameter is T;
	}

	public sealed class IsType<T> : DecoratedSpecification<object>
	{
		public static IsType<T> Default { get; } = new IsType<T>();

		IsType() : base(IsType<object, T>.Default) {}
	}
}