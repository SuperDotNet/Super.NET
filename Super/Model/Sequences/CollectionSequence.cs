using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	sealed class CollectionSequence<_, T> : ISequence<_, T>
	{
		readonly ISelect<_, ICollection<T>> _select;
		readonly IBuilder<T>                _builder;

		public CollectionSequence(ISelect<_, ICollection<T>> select) : this(select, ArrayBuilder<T>.Default) {}

		public CollectionSequence(ISelect<_, ICollection<T>> select, IBuilder<T> builder)
		{
			_select  = @select;
			_builder = builder;
		}

		public ISelect<_, T[]> Get() => _builder.Get()
		                                        .To(I<CollectionArraySelector<T>>.Default)
		                                        .To(_select.Select);

		public ISequence<_, T> Get(IAlterSelection parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISegment<T> parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));
	}

	sealed class CollectionArraySelector<T> : ISelect<ICollection<T>, T[]>, IActivateMarker<IArraySelector<T>>
	{
		readonly IStore<T>         _store;
		readonly IArraySelector<T> _selector;

		[UsedImplicitly]
		public CollectionArraySelector(IArraySelector<T> selector) : this(Allotted<T>.Default, selector) {}

		public CollectionArraySelector(IStore<T> store, IArraySelector<T> selector)
		{
			_store    = store;
			_selector = selector;
		}

		public T[] Get(ICollection<T> parameter)
		{
			var store = _store.Get(parameter.Count);
			parameter.CopyTo(store.Instance, 0);
			using (var session = new Session<T>(store, _store))
			{
				return _selector.Get(session.Store);
			}
		}
	}
}