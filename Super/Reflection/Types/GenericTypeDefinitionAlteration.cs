using System;
using Super.Model.Selection.Alterations;

namespace Super.Reflection.Types
{
	sealed class GenericTypeDefinitionAlteration : DelegatedAlteration<Type>
	{
		public static GenericTypeDefinitionAlteration Default { get; } = new GenericTypeDefinitionAlteration();

		GenericTypeDefinitionAlteration() : base(x => x.GetGenericTypeDefinition()) {}
	}
}