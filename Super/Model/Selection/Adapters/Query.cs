using Super.Model.Results;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Model.Sequences.Query.Construction;

namespace Super.Model.Selection.Adapters
{
	public class Query<_, T> : IResult<ISelect<_, Array<T>>>
	{
		readonly INode<_, T> _node;

		public Query(ISelect<_, T[]> subject) : this(new StartNode<_, T>(subject)) {}

		public Query(ISelect<_, Sequences.Store<T>> subject) : this(new Node<_, T>(subject)) {}

		public Query(INode<_, T> node) => _node = node;

		public ISelect<_, Array<T>> Get() => Out().Select(Sequences.Result<T>.Default);

		public Query<_, T> Select(IPartition partition) => new Query<_, T>(_node.Get(partition));

		public Query<_, T> Select(IBodyBuilder<T> builder) => new Query<_, T>(_node.Get(builder));

		public Query<_, TOut> Select<TOut>(IContents<T, TOut> select) => new Query<_, TOut>(_node.Get(select));

		public ISelect<_, TTo> Select<TTo>(IReduce<T, TTo> select) => _node.Get(select);

		public ISelect<_, T[]> Out() => _node.Get();
	}
}