using System;
using Super.Model.Selection;

namespace Super.Runtime.Environment
{
	sealed class ComponentType : Select<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.Query().FirstAssigned()) {}
	}
}