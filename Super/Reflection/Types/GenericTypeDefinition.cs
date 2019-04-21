using Super.Model.Selection.Alterations;
using System;

namespace Super.Reflection.Types
{
	sealed class GenericTypeDefinition : DelegatedAlteration<Type>
	{
		public static GenericTypeDefinition Default { get; } = new GenericTypeDefinition();

		GenericTypeDefinition() : base(x => x.GetGenericTypeDefinition()) {}
	}
}