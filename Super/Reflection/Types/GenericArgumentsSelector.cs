using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	sealed class GenericArgumentsSelector : Select<TypeInfo, Type[]>
	{
		public static GenericArgumentsSelector Default { get; } = new GenericArgumentsSelector();

		GenericArgumentsSelector() : base(x => x.GenericTypeArguments) {}
	}
}