using Super.ExtensionMethods;
using Super.Model.Commands;

namespace Super.Model.Sources.Tables
{
	class TableAssignment<TParameter, TResult> : ICommand<TResult>
	{
		readonly TParameter                  _key;
		readonly ITable<TParameter, TResult> _source;

		public TableAssignment(TParameter key, ITable<TParameter, TResult> source)
		{
			_key    = key;
			_source = source;
		}

		public void Execute(TResult parameter) => _source.Assign(_key, parameter);
	}
}