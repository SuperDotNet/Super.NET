using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	sealed class GenericParametersSelector : Delegated<TypeInfo, Type[]>
	{
		public static GenericParametersSelector Default { get; } = new GenericParametersSelector();

		GenericParametersSelector() : base(x => x.GenericTypeParameters) {}
	}
}