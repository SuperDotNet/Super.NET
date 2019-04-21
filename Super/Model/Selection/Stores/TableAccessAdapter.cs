using System;

namespace Super.Model.Selection.Stores
{
	public sealed class TableAccessAdapter<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, TOut>                            _create;
		readonly Func<TIn, Func<TIn, TOut>, TOut> _factory;

		public TableAccessAdapter(Func<TIn, Func<TIn, TOut>, TOut> factory,
		                          Func<TIn, TOut> create)
		{
			_factory = factory;
			_create  = create;
		}

		public TOut Get(TIn parameter) => _factory(parameter, _create);
	}
}