using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Sources;
using System;

namespace Super.Model.Sequences
{
	public interface IAlterState<T> : IAlteration<T> {}

	public interface IBuilder<T> : ISource<IArraySelector<T>>,
	                               ISelect<IAlterState<Collections.Selection>, IBuilder<T>>,
	                               ISelect<ISegment<T>, IBuilder<T>> {}

	sealed class Builder<T> : FixedSelection<IStore<T>, IArraySelector<T>>, IBuilder<T>
	{
		public static Builder<T> Default { get; } = new Builder<T>();

		Builder() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public Builder(IArraySelectors<T> selectors) : base(selectors, Allocated<T>.Default)
			=> _selectors = selectors;

		public IBuilder<T> Get(IAlterState<Collections.Selection> parameter)
			=> new Builder<T>(_selectors.Get(parameter));

		public IBuilder<T> Get(ISegment<T> parameter)
			=> new Builder<T>(new SegmentedArraySelectors<T>(_selectors, parameter.Get));
	}

	public interface IArraySelectors<T> : ISelect<IAlterState<Collections.Selection>, IArraySelectors<T>>,
	                                      ISelect<IStore<T>, IArraySelector<T>>,
	                                      ISource<Selection<T>> {}

	sealed class SegmentedArraySelectors<T> : IArraySelectors<T>
	{
		readonly IArraySelector<T>  _previous;
		readonly Selection<T>       _segment;
		readonly IArraySelectors<T> _selectors;

		public SegmentedArraySelectors(IArraySelectors<T> selectors, Selection<T> segment)
			: this(selectors.Get(Allotted<T>.Default), segment, selectors) {}

		public SegmentedArraySelectors(IArraySelector<T> previous, Selection<T> segment, IArraySelectors<T> selectors)
		{
			_previous  = previous;
			_segment   = segment;
			_selectors = selectors;
		}

		public IArraySelectors<T> Get(IAlterState<Collections.Selection> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _segment, _selectors.Get(parameter));

		public IArraySelector<T> Get(IStore<T> parameter)
			=> new LinkedSelector<T>(_previous, new SegmentSelector<T>(Get(), new Copy<T>(parameter).Get));

		public Selection<T> Get() => new LinkedSegment<T>(_segment, _selectors.Get()).Get;
	}

	sealed class LinkedSegment<T> : ISegment<T>
	{
		readonly Selection<T> _previous, _select;

		public LinkedSegment(Selection<T> previous, Selection<T> select)
		{
			_previous = previous;
			_select   = @select;
		}

		public ArrayView<T> Get(in ArrayView<T> parameter) => _select(_previous(in parameter));
	}

	sealed class SegmentSelector<T> : ISelect<T>
	{
		readonly Selection<T>                 _segment;
		readonly Selection<ArrayView<T>, T[]> _select;

		public SegmentSelector(Selection<T> segment, Selection<ArrayView<T>, T[]> select)
		{
			_segment = segment;
			_select  = @select;
		}

		public T[] Get(in ArrayView<T> parameter) => _select(_segment(in parameter));
	}

	public delegate ArrayView<T> Selection<T>(in ArrayView<T> parameter);

	sealed class ArraySelectors<T> : IArraySelectors<T>
	{
		public static ArraySelectors<T> Default { get; } = new ArraySelectors<T>();

		ArraySelectors() : this(Collections.Selection.Default) {}

		readonly Collections.Selection _selection;

		public ArraySelectors(Collections.Selection selection) => _selection = selection;

		public IArraySelectors<T> Get(IAlterState<Collections.Selection> parameter)
			=> new ArraySelectors<T>(parameter.Get(_selection));

		public IArraySelector<T> Get(IStore<T> parameter) => new ArraySelector<T>(parameter, _selection);

		public Selection<T> Get() => new Segment<T>(_selection).Get;
	}

	sealed class Sessions<T> : ISelect<T[], Session<T>>
	{
		public static Sessions<T> Default { get; } = new Sessions<T>();

		Sessions() : this(Allotted<T>.Default) {}

		readonly IStore<T> _store;

		public Sessions(IStore<T> store) => _store = store;

		public Session<T> Get(T[] parameter) => new Session<T>(parameter, _store);
	}

	public interface ISelect<T> : IStructure<ArrayView<T>, T[]> {}

	sealed class Segment<T> : ISegment<T>
	{
		readonly Collections.Selection _selection;

		public Segment(Collections.Selection selection) => _selection = selection;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var size   = _selection.Length.IsAssigned ? _selection.Length.Instance : parameter.Length - _selection.Start;
			var result = new ArrayView<T>(parameter.Array, _selection.Start, size);
			return result;
		}
	}

	sealed class Copy<T> : ISelect<T>
	{
		/*public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allocated<T>.Default) {}*/

		readonly Func<uint, T[]> _source;

		public Copy(IStore<T> store) : this(store.Get) {}

		public Copy(Func<uint, T[]> source) => _source = source;

		public T[] Get(in ArrayView<T> parameter)
			=> parameter.Array.CopyInto(_source(parameter.Length), parameter.Start, parameter.Length);
	}

	/*sealed class ArraySelection<T> : IArraySelector<T>
	{
		public static ArraySelection<T> Default { get; } = new ArraySelection<T>();

		ArraySelection() : this(Copy<T>.Default, Collections.Selection.Default) {}

		readonly ISelect<T>            _select;
		readonly Collections.Selection _selection;

		public ArraySelection(Collections.Selection selection) : this(Copy<T>.Default, selection) {}

		public ArraySelection(ISelect<T> @select) : this(@select, Collections.Selection.Default) {}

		public ArraySelection(ISelect<T> @select, Collections.Selection selection)
		{
			_select    = @select;
			_selection = selection;
		}

		public T[] Get(T[] parameter)
		{
			var size   = _selection.Length.IsAssigned ? _selection.Length.Instance : (uint)parameter.Length;
			var view   = new ArrayView<T>(parameter, _selection.Start, size);
			var result = _select.Get(in view);
			return result;
		}
	}*/

	sealed class LinkedSelector<T> : IArraySelector<T>
	{
		readonly Func<T[], Session<T>> _previous;
		readonly ISelect<T>            _selector;

		public LinkedSelector(IArraySelector<T> previous, ISelect<T> selector)
			: this(previous.Select(Sessions<T>.Default).Get, selector) {}

		public LinkedSelector(Func<T[], Session<T>> previous, ISelect<T> selector)
		{
			_previous = previous;
			_selector = selector;
		}

		public T[] Get(T[] parameter)
		{
			using (var session = _previous(parameter))
			{
				var view = new ArrayView<T>(session.Array, 0, session.Length);
				return _selector.Get(in view);
			}
		}
	}
}