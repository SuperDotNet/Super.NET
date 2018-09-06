using JetBrains.Annotations;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class HasGenericInterface : DecoratedSpecification<TypeInfo>, IActivateMarker<TypeInfo>
	{
		[UsedImplicitly]
		public HasGenericInterface(TypeInfo type)
			: base(I<GenericInterfaceImplementations>.Default.From(type).Out(x => x.Any())) {}
	}
}