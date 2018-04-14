using System.Reflection;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Reflection
{
	public sealed class IsAssignableFrom<T> : DecoratedSpecification<TypeInfo>
	{
		public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

		IsAssignableFrom() : base(new IsAssignableFrom(Type<T>.Metadata)) {}
	}

	public sealed class IsAssignableFrom : DelegatedSpecification<TypeInfo>, IActivateMarker<TypeInfo>
	{
		public IsAssignableFrom(TypeInfo type) : base(type.IsAssignableFrom) {}
	}
}