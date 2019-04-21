using Super.Model.Commands;

namespace Super.Model.Selection.Stores
{
	class TableAssignment<TIn, TOut> : ICommand<TOut>
	{
		readonly TIn                  _key;
		readonly ITable<TIn, TOut> _source;

		public TableAssignment(TIn key, ITable<TIn, TOut> source)
		{
			_key    = key;
			_source = source;
		}

		public void Execute(TOut parameter) => _source.Assign(_key, parameter);
	}
}