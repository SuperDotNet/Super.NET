using System;
using Super.Model.Selection.Alterations;

namespace Super.Reflection.Types
{
	sealed class GenericTypeDefinition : Alteration<Type>
	{
		public static GenericTypeDefinition Default { get; } = new GenericTypeDefinition();

		GenericTypeDefinition() : base(x => x.GetGenericTypeDefinition()) {}
	}
}