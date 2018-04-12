using System;
using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	public sealed class InstanceTypeSelector<T> : Delegated<T, Type>
	{
		public static InstanceTypeSelector<T> Default { get; } = new InstanceTypeSelector<T>();

		InstanceTypeSelector() : base(x => x.GetType()) {}
	}
}