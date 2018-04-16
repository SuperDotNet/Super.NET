using System;
using Super.Model.Selection.Stores;

namespace Super.Model.Selection
{
	sealed class Delegates<TParameter, TResult>
		: Select<ISelect<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static Delegates<TParameter, TResult> Default { get; } = new Delegates<TParameter, TResult>();

		Delegates()
			: base(DefaultReferenceValueTables<ISelect<TParameter, TResult>, Func<TParameter, TResult>>
			       .Default.Get(x => x.Get)
			       .Get) {}
	}
}