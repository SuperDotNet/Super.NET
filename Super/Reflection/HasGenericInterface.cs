using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class HasGenericInterface : ISpecification<TypeInfo>, IActivateMarker<TypeInfo>
	{
		readonly Func<TypeInfo, ImmutableArray<TypeInfo>> _implementations;

		public HasGenericInterface(TypeInfo type) : this(I<GenericInterfaceImplementations>.Default.From(type).Get) {}

		public HasGenericInterface(Func<TypeInfo, ImmutableArray<TypeInfo>> implementations)
			=> _implementations = implementations;

		public bool IsSatisfiedBy(TypeInfo parameter) => _implementations(parameter).Any();
	}
}