using System;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Sequences;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypes : Assume<Type, Array<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(A.This(ComponentTypesDefinition.Default)
		                         .Select(x => x.ToStore())
		                         .ToContextual()
		                         .AsDefined()
		                         .Then()
		                         .Delegate()
		                         .Selector()) {}
	}
}