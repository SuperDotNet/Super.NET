using System.Reflection;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Reflection
{
	public sealed class IsAssignableSpecification<T> : DecoratedSpecification<TypeInfo>
	{
		public static IsAssignableSpecification<T> Default { get; } = new IsAssignableSpecification<T>();

		IsAssignableSpecification() : base(new IsAssignableSpecification(Types<T>.Key)) {}
	}

	public sealed class IsAssignableSpecification : DelegatedSpecification<TypeInfo>, IActivateMarker<TypeInfo>
	{
		public IsAssignableSpecification(TypeInfo type) : base(type.IsAssignableFrom) {}
	}
}