using Super.Model.Collections;
using Super.Model.Selection;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	sealed class CollectionSequence<_, T> : ISequence<_, T>
	{
		readonly ISelect<_, ICollection<T>> _select;
		readonly IBuilder<ICollection<T>, T> _builder;

		public CollectionSequence(ISelect<_, ICollection<T>> select) : this(select, CollectionArrayBuilder<T>.Default) {}

		public CollectionSequence(ISelect<_, ICollection<T>> select, IBuilder<ICollection<T>, T> builder)
		{
			_select  = @select;
			_builder = builder;
		}

		public ISelect<_, T[]> Get() => _select.Select(_builder.Get());

		public ISequence<_, T> Get(IState<Collections.Selection> parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISegment<T> parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));
	}

	public interface ICollectionBuilder<T> : IBuilder<ICollection<T>, T> {}

	sealed class CollectionArrayBuilder<T> : ICollectionBuilder<T>
	{
		public static CollectionArrayBuilder<T> Default { get; } = new CollectionArrayBuilder<T>();

		CollectionArrayBuilder() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public CollectionArrayBuilder(IArraySelectors<T> selectors)
			=> _selectors = selectors;

		public IBuilder<ICollection<T>, T> Get(IState<Collections.Selection> parameter)
			=> new CollectionArrayBuilder<T>(_selectors.Get(parameter));

		public IBuilder<ICollection<T>, T> Get(ISegment<T> parameter)
			=> new CollectionArrayBuilder<T>(new SegmentedArraySelectors<T>(_selectors, parameter));

		public ISelector<ICollection<T>, T> Get() => new CollectionArraySelector<T>(_selectors.Get()
		                                                                                      .Select(Copy<T>.Default)
		                                                                                      .Get);
	}

	sealed class CollectionArraySelector<T> : ISelector<ICollection<T>, T>
	{
		readonly IStore<T> _store;
		readonly Result<T> _result;

		public CollectionArraySelector(Result<T> result) : this(Allotted<T>.Default, result) {}

		public CollectionArraySelector(IStore<T> store, Result<T> result)
		{
			_store  = store;
			_result = result;
		}

		public T[] Get(ICollection<T> parameter)
		{
			using (var session = new Session<T>(_store.Get(parameter.Count), _store, (uint)parameter.Count))
			{
				parameter.CopyTo(session.Array, 0);
				var view = new ArrayView<T>(session.Array, 0, session.Length);
				return _result(in view);
			}
		}
	}
}