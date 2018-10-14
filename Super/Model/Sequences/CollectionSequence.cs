using JetBrains.Annotations;
using Super.Model.Selection;
using Super.Model.Sequences.Query;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	sealed class CollectionSequence<_, T> : DecoratedSource<ISelect<_, T[]>>, ISequence<_, T>
	{
		readonly ISelect<_, ICollection<T>> _select;
		readonly IBuilder<T>                _builder;

		public CollectionSequence(ISelect<_, ICollection<T>> select) : this(select, ArrayBuilder<T>.Default) {}

		public CollectionSequence(ISelect<_, ICollection<T>> select,
		                          IBuilder<T> builder) : base(builder.Select(I<CollectionArrayResult<T>>.Default)
		                                                             .Select(select.Select))
		{
			_select  = @select;
			_builder = builder;
		}

		public ISequence<_, T> Get(IAlterSelection parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));

		public ISequence<_, T> Get(ISelectView<T> parameter)
			=> new CollectionSequence<_, T>(_select, _builder.Get(parameter));

		public ISelect<_, T> Get(IElement<T> parameter)
			=> _builder.Get(parameter).To(I<CollectionElementResult<T>>.Default).To(_select.Select);
	}

	sealed class CollectionArrayResult<T> : CollectionSelection<T, T[]>, IActivateMarker<IArraySelector<T>>
	{
		public CollectionArrayResult(IArraySelector<T> selector) : base(selector) {}
	}

	sealed class CollectionElementResult<T> : CollectionSelection<T, T>, IActivateMarker<ISelect<Store<T>, T>>
	{
		public CollectionElementResult(ISelect<Store<T>, T> selector) : base(selector) {}
	}

	class CollectionSelection<T, TResult> : ISelect<ICollection<T>, TResult>
	{
		readonly IStore<T>                  _store;
		readonly ISelect<Store<T>, TResult> _selector;

		[UsedImplicitly]
		public CollectionSelection(ISelect<Store<T>, TResult> selector)
			: this(Allotted<T>.Default, selector) {}

		public CollectionSelection(IStore<T> store, ISelect<Store<T>, TResult> selector)
		{
			_store    = store;
			_selector = selector;
		}

		public TResult Get(ICollection<T> parameter)
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