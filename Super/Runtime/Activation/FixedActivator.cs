using System;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Runtime.Activation
{
	public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>
	{
		public FixedActivator(ISelect<Type, T> select) : base(select, Type<T>.Instance) {}
	}
}