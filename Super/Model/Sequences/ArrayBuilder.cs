using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sequences
{
	public interface IBuilder<in TIn, T> : ISource<ISelector<TIn, T>>,
	                                       ISelect<IAlterSelection, IBuilder<TIn, T>>,
	                                       ISelect<ISegment<T>, IBuilder<TIn, T>> {}

	public interface IBuilder<T> : ISource<IArraySelector<T>>,
	                               ISelect<IAlterSelection, IBuilder<T>>,
	                               ISelect<ISegment<T>, IBuilder<T>> {}

	public interface IAlterNode<T> : ISelect<IAlterSelection, IAlterNode<T>>,
	                                 IAlteration<ISegment<T>>,
	                                 ISource<IArraySelector<T>> {}

	sealed class AlterNode<T> : Source<IArraySelector<T>>, IAlterNode<T>, IActivateMarker<INode<T>>
	{
		/*readonly static Func<IAlteration<Collections.Selection>, AlterNode<T>> Selectors
			= Selections<T>.Default.Select(I<AlterNode<T>>.Default.From).Get;*/

		public static AlterNode<T> Default { get; } = new AlterNode<T>();

		AlterNode() : this(Node<T>.Default) {}

		readonly INode<T> _node;

		public AlterNode(INode<T> node) : base(node.Get()) => _node = node;

		public IAlterNode<T> Get(IAlterSelection parameter) => new AlterNode<T>(_node.Get(parameter));

		public ISegment<T> Get(ISegment<T> parameter) => _node.Get(parameter);
	}

	sealed class SegmentedArrayBuilder<T> : IBuilder<T>, IActivateMarker<ISegment<T>>
	{
		readonly ISelect<ArrayView<T>, Store<T>> _previous;
		readonly IAlterNode<T>                      _alter;

		[UsedImplicitly]
		public SegmentedArrayBuilder(ISegment<T> segment) : this(segment.Store(), AlterNode<T>.Default) {}

		public SegmentedArrayBuilder(ISelect<ArrayView<T>, Store<T>> previous, IAlterNode<T> alter)
		{
			_previous = previous;
			_alter    = alter;
		}

		public IArraySelector<T> Get() => new DelegatedArraySelector<T>(_previous.Select(_alter.Get()));

		public IBuilder<T> Get(IAlterSelection parameter)
			=> new SegmentedArrayBuilder<T>(_previous, _alter.Get(parameter));

		public IBuilder<T> Get(ISegment<T> parameter) => _alter.Get(parameter)
		                                                       .To(I<SegmentedArrayBuilder<T>>.Default.From);
	}

	sealed class ArrayBuilder<T> : DecoratedSource<IArraySelector<T>>, IBuilder<T>, IActivateMarker<IAlterNode<T>>
	{
		public static ArrayBuilder<T> Default { get; } = new ArrayBuilder<T>();

		ArrayBuilder() : this(AlterNode<T>.Default) {}

		readonly IAlterNode<T> _alter;

		public ArrayBuilder(IAlterNode<T> alter) : base(alter) => _alter = alter;

		public IBuilder<T> Get(ISegment<T> parameter) => _alter.Get(parameter)
		                                                       .To(I<SegmentedArrayBuilder<T>>.Default.From);

		public IBuilder<T> Get(IAlterSelection parameter) => _alter.Get(parameter).To(I<ArrayBuilder<T>>.Default.From);
	}

	public interface IAlterSelection : IAlteration<Collections.Selection> {}

	/*public interface IArraySelectors<T> : ISelect<IAlterSelection, IArraySelectors<T>>,
	                                      ISelect<ISessions<T>, IStructure<ArrayView<T>, Session<T>>>,
	                                      ISelect<ISession<T>, IStructure<ArrayView<T>, T[]>>,
	                                      ISource<IArraySelector<T>> {}*/

	/*sealed class SegmentedArraySelectors<T> : ISegmented<T>
	{
		readonly IStructure<ArrayView<T>, Session<T>> _previous;
		readonly ISegment<T>                          _segment;
		readonly ISegmented<T>                        _selectors;

		public SegmentedArraySelectors(IStructure<ArrayView<T>, Session<T>> previous, ISegment<T> segment)
			: this(previous, Segmented<T>.Default, segment) {}

		public SegmentedArraySelectors(IStructure<ArrayView<T>, Session<T>> previous, IArraySelectors<T> selectors,
		                               ISegment<T> segment) : this(previous, new Segmented<T>(selectors), segment) {}

		public SegmentedArraySelectors(IStructure<ArrayView<T>, Session<T>> previous, ISegmented<T> selectors,
		                               ISegment<T> segment)
		{
			_previous  = previous;
			_segment   = segment;
			_selectors = selectors;
		}

		public IArraySelectors<T> Get(IAlterSelection parameter)
			=> new SegmentedArraySelectors<T>(_previous, _selectors.Get(parameter), _segment);

		public IArraySelectors<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _selectors.Get(parameter), _segment);

		public IArraySelector<T> Get()
			=> new LinkedArraySelector<T>(_previous.Get, _segment.Select(_selectors.Get(Copy<T>.Default)).Get);

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter)
			=> new LinkedSegment<T>(_previous.Get, _segment.Get).Select(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter)
			=> new LinkedResult<T>(_previous.Get, _segment.Select(_selectors.Get(parameter)).Get);
	}*/

	/*public interface ISegmented<T> : IArraySelectors<T>, ISelect<ISegment<T>, IArraySelectors<T>> {}*/

	/*sealed class Segmented<T> : ISegmented<T>
	{
		public static Segmented<T> Default { get; } = new Segmented<T>();

		Segmented() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public Segmented(IArraySelectors<T> selectors) => _selectors = selectors;

		public IArraySelectors<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_selectors.Get(Sessions<T>.Default), parameter);

		public IArraySelectors<T> Get(IAlterSelection parameter) => _selectors.Get(parameter);

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter) => _selectors.Get(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter) => _selectors.Get(parameter);

		public IArraySelector<T> Get() => _selectors.Get();
	}*/

	/*sealed class ArraySelectors<T> : Source<IArraySelector<T>>, IArraySelectors<T>
	{
		public static ArraySelectors<T> Default { get; } = new ArraySelectors<T>();

		ArraySelectors() : this(Collections.Selection.Default) {}

		readonly ISegment<T>           _segment;
		readonly Collections.Selection _selection;

		public ArraySelectors(Collections.Selection selection) : this(new SelectedSegment<T>(selection), selection) {}

		public ArraySelectors(ISegment<T> segment, Collections.Selection selection) :
			base(new ArraySelector<T>(selection))
		{
			_segment   = segment;
			_selection = selection;
		}

		public IArraySelectors<T> Get(IAlterSelection parameter) => new ArraySelectors<T>(parameter.Get(_selection));

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter) => _segment.Select(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter) => _segment.Select(parameter);
	}*/

	/*sealed class LinkedSegment<T> : ISegment<T>
	{
		readonly Result<ArrayView<T>, Session<T>>   _previous;
		readonly Result<ArrayView<T>, ArrayView<T>> _select;

		public LinkedSegment(Result<ArrayView<T>, Session<T>> previous, Result<ArrayView<T>, ArrayView<T>> select)
		{
			_previous = previous;
			_select   = @select;
		}

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			using (var session = _previous(in parameter))
			{
				return _select(session.Store);
			}
		}
	}*/

	sealed class SelectedSegment<T> : ISegment<T>
	{
		public static SelectedSegment<T> Default { get; } = new SelectedSegment<T>();

		SelectedSegment() : this(Collections.Selection.Default) {}

		readonly uint           _start;
		readonly Assigned<uint> _length;

		public SelectedSegment(Collections.Selection selection) : this(selection.Start, selection.Length) {}

		public SelectedSegment(uint start, Assigned<uint> length)
		{
			_start  = start;
			_length = length;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var size   = _length.IsAssigned ? _length.Instance : parameter.Length - _start;
			var result = new ArrayView<T>(parameter.Array, _start, size);
			return result;
		}
	}

	public interface ISessions<T> : ISelect<ArrayView<T>, Session<T>> {}

	sealed class Sessions<T> : ISessions<T>
	{
		public static Sessions<T> Default { get; } = new Sessions<T>();

		Sessions() : this(Allotted<T>.Default) {}

		readonly IStore<T>                      _store;
		readonly Func<ArrayView<T>, Store<T>> _result;

		public Sessions(IStore<T> store) : this(store, new Copy<T>(store).Get) {}

		public Sessions(IStore<T> store, Func<ArrayView<T>, Store<T>> result)
		{
			_store  = store;
			_result = result;
		}

		public Session<T> Get(ArrayView<T> parameter) => new Session<T>(_result(parameter), _store);
	}

	public interface ISession<T> : ISelect<ArrayView<T>, Store<T>> {}

	sealed class Copy<T> : ISession<T>
	{
		public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allocated<T>.Default) {}

		readonly IStore<T> _store;

		public Copy(IStore<T> store) => _store = store;

		public Store<T> Get(ArrayView<T> parameter)
		{
			var input = _store.Get(parameter.Length);
			return parameter.Array.CopyInto(in input, parameter.Start);
		}
	}

	sealed class DelegatedArraySelector<T> : IArraySelector<T>, IActivateMarker<ISelect<ArrayView<T>, T[]>>
	{
		readonly Func<ArrayView<T>, T[]> _continue;

		/*public DelegatedArraySelector(IStructure<ArrayView<T>, Store<T>> @continue)
			: this(@continue.Select((in Store<T> x) => x.Instance).Get) {}*/

		public DelegatedArraySelector(ISelect<ArrayView<T>, T[]> result) : this(result.Get) {}

		public DelegatedArraySelector(Func<ArrayView<T>, T[]> result) => _continue = result;

		public T[] Get(Store<T> parameter) => _continue(parameter);
	}

	sealed class SessionSegment<T> : ISegment<T>
	{
		readonly Func<ArrayView<T>, Session<T>>   _previous;
		readonly Func<ArrayView<T>, ArrayView<T>> _continue;

		public SessionSegment(Func<ArrayView<T>, Session<T>> previous, Func<ArrayView<T>, ArrayView<T>> @continue)
		{
			_previous = previous;
			_continue = @continue;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			using (var session = _previous(parameter))
			{
				return _continue(session.Store);
			}
		}
	}
}