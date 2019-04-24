using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public class SelectedInstanceSelector<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, Func<TIn, TOut>> _select;

		public SelectedInstanceSelector(Func<TIn, Func<TIn, TOut>> select) => _select = @select;

		public TOut Get(TIn parameter) => _select(parameter)(parameter);
	}

	public class Assume<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<Func<Func<TIn, TOut>>>
	{
		readonly Func<Func<TIn, TOut>> _source;

		public Assume(Func<Func<TIn, TOut>> source) => _source = source;

		public TOut Get(TIn parameter) => _source()(parameter);
	}
}