using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class DecoratedTable<TParameter, TResult> : Decorated<TParameter, TResult>, ITable<TParameter, TResult>
	{
		readonly ITable<TParameter, TResult> _source;

		public DecoratedTable(ITable<TParameter, TResult> source) : base(source) => _source = source;

		public bool IsSatisfiedBy(TParameter parameter) => _source.IsSatisfiedBy(parameter);

		public void Execute(KeyValuePair<TParameter, TResult> parameter) => _source.Execute(parameter);

		public bool Remove(TParameter key) => _source.Remove(key);
	}
}