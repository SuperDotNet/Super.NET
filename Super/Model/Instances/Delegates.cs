using Super.Model.Sources;
using System;

namespace Super.Model.Instances
{
	sealed class Delegates<T> : ReferenceStore<IInstance<T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}
