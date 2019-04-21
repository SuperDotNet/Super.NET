using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Results
{
	sealed class Delegates<T> : ReferenceValueStore<IResult<T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}
