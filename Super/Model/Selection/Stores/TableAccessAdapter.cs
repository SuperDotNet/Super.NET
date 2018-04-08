using System;

namespace Super.Model.Selection.Stores
{
	public sealed class TableAccessAdapter<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TParameter, TResult>                            _create;
		readonly Func<TParameter, Func<TParameter, TResult>, TResult> _factory;

		public TableAccessAdapter(Func<TParameter, Func<TParameter, TResult>, TResult> factory,
		                          Func<TParameter, TResult> create)
		{
			_factory = factory;
			_create  = create;
		}

		public TResult Get(TParameter parameter) => _factory(parameter, _create);
	}
}