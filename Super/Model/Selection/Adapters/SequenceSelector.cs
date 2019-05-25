﻿using Super.Model.Collections;
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

		public Query(ISelect<_, T[]> subject) : this(new Start<_, T>(subject)) {}

		public Query(ISelect<_, Sequences.Store<T>> subject) : this(new Node<_, T>(subject)) {}

		public Query(INode<_, T> node) => _node = node;

		public Query<_, T> Select(Sequences.Selection selection) => Skip(selection.Start).Take(selection.Length);

		public Query<_, T> Append(ISequence<T> others) => Select(new Build.Concatenation<T>(others).Returned());

		public Query<_, T> Union(ISequence<T> others) => Union(others, EqualityComparer<T>.Default);

		public Query<_, T> Union(ISequence<T> others, IEqualityComparer<T> comparer)
			=> Select(new Build.Union<T>(others, comparer).Returned());

		public Query<_, T> Intersect(ISequence<T> others) => Intersect(others, EqualityComparer<T>.Default);

		public Query<_, T> Intersect(ISequence<T> others, IEqualityComparer<T> comparer)
			=> Select(new Build.Intersect<T>(others, comparer).Returned());

		public Query<_, T> Distinct() => new Query<_, T>(_node.Get(Build.Distinct<T>.Default));

		public Query<_, T> Distinct(IEqualityComparer<T> comparer)
			=> new Query<_, T>(_node.Get(new Build.Distinct<T>(comparer)));

		public Query<_, T> Skip(uint count) => Select(new Skip(count));

		public Query<_, T> Take(uint count) => Select(new Take(count));

		public Query<_, T> Where(ISelect<T, bool> where) => Where(where.Get);

		public Query<_, T> Where(Func<T, bool> where) => new Query<_, T>(_node.Get(new Build.Where<T>(where)));


		public Query<_, T> Select(IBodyBuilder<T> builder) => new Query<_, T>(_node.Get(builder));

		public Query<_, T> Select(IPartition partition) => new Query<_, T>(_node.Get(partition));


		public Query<_, TOut> Select<TOut>(IContents<T, TOut> select) => new Query<_, TOut>(_node.Get(select));

		public ISelect<_, T> Select(IElement<T> select) => _node.Get(select);

		public ISelect<_, T> Only() => Select(Only<T>.Default);

		public ISelect<_, T> Only(Func<T, bool> where) => new Only<T>(where).To(Select);

		public ISelect<_, T> First() => Select(FirstOrDefault<T>.Default);

		public ISelect<_, T> First(Func<T, bool> where) => new FirstWhere<T>(where).To(Select);

		public ISelect<_, T> Single() => Select(Single<T>.Default);

		public ISelect<_, T> Single(Func<T, bool> where) => new Single<T>(where).To(Select);

		public ISelect<_, TTo> Select<TTo>(IElement<T, TTo> select) => _node.Get(select);

		public ISelect<_, int> Sum(Func<T, int> select) => Select(new Sum32<T>(select));

		public ISelect<_, uint> Sum(Func<T, uint> select) => Select(new SumUnsigned32<T>(select));

		public ISelect<_, long> Sum(Func<T, long> select) => Select(new Sum64<T>(select));

		public ISelect<_, ulong> Sum(Func<T, ulong> select) => Select(new SumUnsigned64<T>(select));

		public ISelect<_, float> Sum(Func<T, float> select) => Select(new SumSingle<T>(select));

		public ISelect<_, double> Sum(Func<T, double> select) => Select(new SumDouble<T>(select));

		public ISelect<_, decimal> Sum(Func<T, decimal> select) => Select(new SumDecimal<T>(select));

		public ISelect<_, T[]> Out() => _node.Get();

		public ISelect<_, Array<T>> Get() => Out().Select(Sequences.Result<T>.Default);
	}
}