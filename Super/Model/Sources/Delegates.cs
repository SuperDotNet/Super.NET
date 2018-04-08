using System;
using Super.Model.Selection.Stores;

namespace Super.Model.Sources
{
	sealed class Delegates<T> : ReferenceStore<ISource<T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}
