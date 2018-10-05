using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface IState<T> : IAlteration<T> {}

	public interface IBuilder<in TIn, T> : ISource<ISelector<TIn, T>>,
	                                       ISelect<IState<Collections.Selection>, IBuilder<TIn, T>>,
	                                       ISelect<ISegment<T>, IBuilder<TIn, T>> {}

	public interface IBuilder<T> : IBuilder<T[], T> {}

	sealed class ArrayBuilder<T> : IBuilder<T>
	{
		public static ArrayBuilder<T> Default { get; } = new ArrayBuilder<T>();

		ArrayBuilder() : this(ArraySelectors<T>.Default) {}

		readonly ISegmented<T> _segmented;

		public ArrayBuilder(IArraySelectors<T> selectors) : this(new Segmented<T>(selectors)) {}

		public ArrayBuilder(ISegmented<T> segmented) => _segmented = segmented;

		public IBuilder<T[], T> Get(IState<Collections.Selection> parameter)
			=> new ArrayBuilder<T>(_segmented.Get(parameter));

		public IBuilder<T[], T> Get(ISegment<T> parameter) => new ArrayBuilder<T>(_segmented.Get(parameter));

		public ISelector<T[], T> Get() => _segmented.Get();
	}

	public interface IArraySelectors<T> : ISelect<IState<Collections.Selection>, IArraySelectors<T>>,
	                                      ISelect<ISessions<T>, IStructure<ArrayView<T>, Session<T>>>,
	                                      ISelect<IResult<T>, IStructure<ArrayView<T>, T[]>>,
	                                      ISource<ISelector<T>> {}

	sealed class SegmentedArraySelectors<T> : ISegmented<T>
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

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _selectors.Get(parameter), _segment);

		public IArraySelectors<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _selectors.Get(parameter), _segment);

		public ISelector<T> Get()
			=> new LinkedSelector<T>(_previous.Get, _segment.Select(_selectors.Get(Copy<T>.Default)).Get);

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter)
			=> new LinkedSegment<T>(_previous.Get, _segment.Get).Select(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter)
			=> new LinkedResult<T>(_previous.Get, _segment.Select(_selectors.Get(parameter)).Get);
	}

	public interface ISegmented<T> : IArraySelectors<T>, ISelect<ISegment<T>, IArraySelectors<T>> {}

	sealed class Segmented<T> : ISegmented<T>
	{
		public static Segmented<T> Default { get; } = new Segmented<T>();

		Segmented() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public Segmented(IArraySelectors<T> selectors) => _selectors = selectors;

		public IArraySelectors<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_selectors.Get(Sessions<T>.Default), parameter);

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter) => _selectors.Get(parameter);

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter) => _selectors.Get(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter) => _selectors.Get(parameter);

		public ISelector<T> Get() => _selectors.Get();
	}

	sealed class ArraySelectors<T> : Source<ISelector<T>>, IArraySelectors<T>
	{
		public static ArraySelectors<T> Default { get; } = new ArraySelectors<T>();

		ArraySelectors() : this(Collections.Selection.Default) {}

		readonly ISegment<T>           _segment;
		readonly Collections.Selection _selection;

		public ArraySelectors(Collections.Selection selection) : this(new SelectedSegment<T>(selection), selection) {}

		public ArraySelectors(ISegment<T> segment, Collections.Selection selection) : base(new Selector<T>(selection))
		{
			_segment   = segment;
			_selection = selection;
		}

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter)
			=> new ArraySelectors<T>(parameter.Get(_selection));

		public IStructure<ArrayView<T>, Session<T>> Get(ISessions<T> parameter) => _segment.Select(parameter);

		public IStructure<ArrayView<T>, T[]> Get(IResult<T> parameter) => _segment.Select(parameter);
	}

	sealed class LinkedSegment<T> : ISegment<T>
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
				var view = new ArrayView<T>(session.Array, 0, session.Length);
				return _select(in view);
			}
		}
	}

	sealed class SelectedSegment<T> : ISegment<T>
	{
		readonly Collections.Selection _selection;

		public SelectedSegment(Collections.Selection selection) => _selection = selection;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var size = _selection.Length.IsAssigned
				           ? _selection.Length.Instance
				           : parameter.Length - _selection.Start;
			var result = new ArrayView<T>(parameter.Array, _selection.Start, size);
			return result;
		}
	}

	public interface ISessions<T> : IStructure<ArrayView<T>, Session<T>> {}

	sealed class Sessions<T> : ISessions<T>
	{
		public static Sessions<T> Default { get; } = new Sessions<T>();

		Sessions() : this(Allotted<T>.Default) {}

		readonly IStore<T>                 _store;
		readonly Result<ArrayView<T>, T[]> _result;

		public Sessions(IStore<T> store) : this(store, new Copy<T>(store).Get) {}

		public Sessions(IStore<T> store, Result<ArrayView<T>, T[]> result)
		{
			_store  = store;
			_result = result;
		}

		public Session<T> Get(in ArrayView<T> parameter)
			=> new Session<T>(_result(parameter), _store, parameter.Length);
	}

	public interface IResult<T> : IStructure<ArrayView<T>, T[]> {}

	sealed class Copy<T> : IResult<T>
	{
		public static Copy<T> Default { get; } = new Copy<T>();

		Copy() : this(Allocated<T>.Default) {}

		readonly IStore<T> _store;

		public Copy(IStore<T> store) => _store = store;

		public T[] Get(in ArrayView<T> parameter)
			=> parameter.Array.CopyInto(_store.Get(parameter.Length), parameter.Start, parameter.Length);
	}

	sealed class LinkedSelector<T> : ISelector<T>
	{
		readonly Result<ArrayView<T>, Session<T>> _previous;
		readonly Result<ArrayView<T>, T[]>        _result;

		public LinkedSelector(Result<ArrayView<T>, Session<T>> previous, Result<ArrayView<T>, T[]> result)
		{
			_previous = previous;
			_result   = result;
		}

		public T[] Get(T[] parameter)
		{
			var view = new ArrayView<T>(parameter);
			using (var session = _previous(in view))
			{
				var selection = new ArrayView<T>(session.Array, 0, session.Length);
				return _result(in selection);
			}
		}
	}

	sealed class LinkedResult<T> : IResult<T>
	{
		readonly Result<ArrayView<T>, Session<T>> _previous;
		readonly Result<ArrayView<T>, T[]>        _result;

		public LinkedResult(Result<ArrayView<T>, Session<T>> previous, Result<ArrayView<T>, T[]> result)
		{
			_previous = previous;
			_result   = result;
		}

		public T[] Get(in ArrayView<T> parameter)
		{
			using (var session = _previous(in parameter))
			{
				var selection = new ArrayView<T>(session.Array, 0, session.Length);
				return _result(in selection);
			}
		}
	}
}