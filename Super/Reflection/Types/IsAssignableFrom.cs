using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class IsAssignableFrom<T> : DecoratedSpecification<TypeInfo>
	{
		public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

		IsAssignableFrom() : base(new IsAssignableFrom(Type<T>.Metadata)) {}
	}

	public sealed class IsAssignableFrom : DelegatedSpecification<Type>, IActivateMarker<Type>
	{
		public IsAssignableFrom(Type type) : base(type.IsAssignableFrom) {}
	}
}