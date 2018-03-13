using System;

namespace Super.Model.Sources.Tables
{
	public sealed class TableAccessAdapter<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TParameter, TResult>                            _create;
		readonly Func<TParameter, Func<TParameter, TResult>, TResult> _factory;

		public TableAccessAdapter(Func<TParameter, Func<TParameter, TResult>, TResult> factory,
		                          Func<TParameter, TResult> create)
		{
			_factory = factory;
			_create  = create;
		}

		public TResult Get(TParameter parameter) => _factory.Invoke(parameter, _create);
	}
}