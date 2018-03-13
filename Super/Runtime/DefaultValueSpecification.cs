using Super.Model.Specifications;

namespace Super.Runtime
{
	sealed class DefaultValueSpecification<T> : EqualitySpecification<T>
	{
		public static DefaultValueSpecification<T> Default { get; } = new DefaultValueSpecification<T>();

		DefaultValueSpecification() : base(default) {}
	}
}