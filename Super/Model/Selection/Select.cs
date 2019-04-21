using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public class Select<TIn, TOut> : ISelect<TIn, TOut>,
	                                 IActivateUsing<Func<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _source;

		public Select(Func<TIn, TOut> source) => _source = source;

		public TOut Get(TIn parameter) => _source(parameter);
	}
}