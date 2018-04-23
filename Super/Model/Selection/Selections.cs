using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Selection
{
	sealed class Selections<TParameter, TResult> : ReferenceStore<Func<TParameter, TResult>, ISelect<TParameter, TResult>>
	{
		public static Selections<TParameter, TResult> Default { get; } = new Selections<TParameter, TResult>();

		Selections() : base(x => x.Target as ISelect<TParameter, TResult> ?? new Select<TParameter, TResult>(x)) {}
	}
}