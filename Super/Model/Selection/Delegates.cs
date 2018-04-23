using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Selection
{
	sealed class Delegates<TParameter, TResult> : ReferenceStore<ISelect<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static Delegates<TParameter, TResult> Default { get; } = new Delegates<TParameter, TResult>();

		Delegates() : base(x => x.Get) {}
	}
}