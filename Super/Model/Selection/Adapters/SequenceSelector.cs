using Super.Model.Collections;
using Super.Model.Results;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Model.Sequences.Query.Construction;
using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Adapters
{
	public class SequenceSelector<_, T> : Selector<_, IEnumerable<T>>
	{
		public SequenceSelector(ISelect<_, IEnumerable<T>> subject) : base(subject) {}
	}

	public class CollectionSelector<_, T> : SequenceSelector<_, T>
	{
		readonly ISelect<_, ICollection<T>> _subject;

		public CollectionSelector(ISelect<_, ICollection<T>> subject) : base(subject) => _subject = subject;

		public ConditionSelector<_> HasAny() => new ConditionSelector<_>(_subject.Select(HasAny<T>.Default));

		public ConditionSelector<_> HasNone() => new ConditionSelector<_>(_subject.Select(HasNone<T>.Default));
	}

	public class OpenArraySelector<_, T> : CollectionSelector<_, T>
	{
		readonly ISelect<_, T[]> _subject;

		public OpenArraySelector(ISelect<_, T[]> subject) : base(subject) => _subject = subject;

		public ConditionSelector<_> AllAre(Func<T, bool> condition)
			=> new ConditionSelector<_>(_subject.Select(new AllItemsAre<T>(condition)));

		public OpenArraySelector<_, T> Sort()
			=> new OpenArraySelector<_, T>(_subject.Select(SortAlteration<T>.Default));
	}

	public class Query<_, T> : IResult<ISelect<_, Array<T>>>
	{
		readonly INode<_, T> _node;

		public Query(ISelect<_, T[]> subject) : this(new StartNode<_, T>(subject)) {}

		public Query(ISelect<_, Sequences.Store<T>> subject) : this(new Node<_, T>(subject)) {}

		public Query(INode<_, T> node) => _node = node;

		public Query<_, T> Select(IPartition partition) => new Query<_, T>(_node.Get(partition));

		public Query<_, T> Select(IBodyBuilder<T> builder) => new Query<_, T>(_node.Get(builder));

		public Query<_, TOut> Select<TOut>(IContents<T, TOut> select) => new Query<_, TOut>(_node.Get(select));

		public ISelect<_, TTo> Select<TTo>(IReduce<T, TTo> select) => _node.Get(select);

		public ISelect<_, T[]> Out() => _node.Get();

		public ISelect<_, Array<T>> Get() => Out().Select(Sequences.Result<T>.Default);
	}
}