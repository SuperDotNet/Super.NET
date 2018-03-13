using System;
using System.Collections.Immutable;
using System.Linq;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class GenericTypeAlteration : ISource<ImmutableArray<Type>, Type>
	{
		readonly Type _definition;

		public GenericTypeAlteration(Type definition) => _definition = definition;

		public Type Get(ImmutableArray<Type> parameter) => _definition.MakeGenericType(parameter.ToArray());
	}
}