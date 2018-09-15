using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Reflection.Types
{
	public class GenericTypeAlteration : ISelect<ImmutableArray<Type>, Type>, IActivateMarker<Type>
	{
		readonly Type _definition;

		public GenericTypeAlteration(Type definition) => _definition = definition;

		public Type Get(ImmutableArray<Type> parameter)
			=> _definition.MakeGenericType(ImmutableArrayExtensions.ToArray(parameter));
	}
}