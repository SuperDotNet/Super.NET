using Super.Model.Specifications;

namespace Super.Runtime
{
	sealed class IsDefault<T> : EqualitySpecification<T>
	{
		public static IsDefault<T> Default { get; } = new IsDefault<T>();

		IsDefault() : base(default) {}
	}
}