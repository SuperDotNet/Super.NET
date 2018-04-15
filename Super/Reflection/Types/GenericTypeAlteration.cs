using System;
using System.Collections.Immutable;
using System.Linq;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	sealed class GenericTypeAlteration : ISelect<ImmutableArray<Type>, Type>
	{
		readonly Type _definition;

		public GenericTypeAlteration(Type definition) => _definition = definition;

		public Type Get(ImmutableArray<Type> parameter) => _definition.MakeGenericType(parameter.ToArray());
	}
}