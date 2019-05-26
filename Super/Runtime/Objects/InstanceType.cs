using System;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class InstanceType<T> : Select<T, Type>
	{
		public static InstanceType<T> Default { get; } = new InstanceType<T>();

		InstanceType() : base(x => x?.GetType() ?? Type<T>.Instance) {}
	}
}