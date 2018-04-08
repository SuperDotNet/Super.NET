using Super.Model.Selection;
using System;

namespace Super.Runtime
{
	sealed class InstanceTypeSelector<T> : Delegated<T, Type>
	{
		public static InstanceTypeSelector<T> Default { get; } = new InstanceTypeSelector<T>();

		InstanceTypeSelector() : base(x => x.GetType()) {}
	}
}