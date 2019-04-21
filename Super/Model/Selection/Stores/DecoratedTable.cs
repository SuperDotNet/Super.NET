using Super.Model.Selection.Conditions;
using Super.Runtime;

namespace Super.Model.Selection.Stores
{
	public class DecoratedTable<TIn, TOut> : ITable<TIn, TOut>
	{
		public ICondition<TIn> Condition { get; }
		readonly ITable<TIn, TOut> _source;

		public DecoratedTable(ITable<TIn, TOut> source) : this(source.Condition, source) {}

		public DecoratedTable(ICondition<TIn> condition, ITable<TIn, TOut> source)
		{
			Condition = condition;
			_source = source;
		}

		public void Execute(Pair<TIn, TOut> parameter) => _source.Execute(parameter);

		public bool Remove(TIn key) => _source.Remove(key);

		public TOut Get(TIn parameter) => _source.Get(parameter);
	}
}