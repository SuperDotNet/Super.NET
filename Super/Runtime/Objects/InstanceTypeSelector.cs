using Super.Model.Selection;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Objects
{
	public sealed class InstanceTypeSelector<T> : Select<T, Type>
	{
		public static InstanceTypeSelector<T> Default { get; } = new InstanceTypeSelector<T>();

		InstanceTypeSelector() : base(x => x?.GetType() ?? Type<T>.Instance) {}
	}
}