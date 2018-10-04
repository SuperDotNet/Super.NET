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
	                                      ISelect<ISessions<T>, Link<T>>,
	                                      ISelect<IResult<T>, Result<T>>,
	                                      ISource<ISelector<T>> {}

	sealed class SegmentedArraySelectors<T> : ISegmented<T>
	{
		readonly Link<T>       _previous;
		readonly ISegment<T>   _segment;
		readonly ISegmented<T> _selectors;

		public SegmentedArraySelectors(Link<T> previous, ISegment<T> segment)
			: this(previous, segment, Segmented<T>.Default) {}

		public SegmentedArraySelectors(Link<T> previous, ISegment<T> segment, IArraySelectors<T> selectors)
			: this(previous, segment, selectors as ISegmented<T> ?? new Segmented<T>(selectors)) {}

		public SegmentedArraySelectors(Link<T> previous, ISegment<T> segment, ISegmented<T> selectors)
		{
			_previous  = previous;
			_segment   = segment;
			_selectors = selectors;
		}

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _segment, _selectors.Get(parameter));

		public ISegmented<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _segment, _selectors.Get(parameter));

		public ISelector<T> Get() => new LinkedSelector<T>(_previous, new Linked<T>(_segment.Get, _selectors.Get(Copy<T>.Default)).Get);

		public Link<T> Get(ISessions<T> parameter)
			=> new LinkedSegment<T>(_previous, _segment.Get).Select(parameter).Get;

		public Result<T> Get(IResult<T> parameter)
			=> new LinkedView<T>(_previous, new Linked<T>(_segment.Get, _selectors.Get(parameter)).Get).Get;
	}

	sealed class Linked<T> : IStructure<ArrayView<T>, T[]>
	{
		readonly Result<T> _result;
		readonly Alter<T>  _alter;

		public Linked(Alter<T> alter, Result<T> result)
		{
			_result = result;
			_alter  = alter;
		}

		public T[] Get(in ArrayView<T> parameter) => _result(_alter(in parameter));
	}


	public interface ISegmented<T> : IArraySelectors<T>, ISelect<ISegment<T>, ISegmented<T>> {}

	sealed class Segmented<T> : ISegmented<T>
	{
		public static Segmented<T> Default { get; } = new Segmented<T>();

		Segmented() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public Segmented(IArraySelectors<T> selectors) => _selectors = selectors;

		public ISegmented<T> Get(ISegment<T> parameter)
			=> new SegmentedArraySelectors<T>(_selectors.Get(Sessions<T>.Default), parameter);

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter) => _selectors.Get(parameter);

		public Link<T> Get(ISessions<T> parameter) => _selectors.Get(parameter);

		public Result<T> Get(IResult<T> parameter) => _selectors.Get(parameter);

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

		public Link<T> Get(ISessions<T> parameter) => _segment.Select(parameter).Get;

		public Result<T> Get(IResult<T> parameter) => _segment.Select(parameter).Get;
	}

	sealed class LinkedSegment<T> : ISegment<T>
	{
		readonly Link<T>  _previous;
		readonly Alter<T> _select;

		public LinkedSegment(Link<T> previous, Alter<T> select)
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

	public delegate ArrayView<T> Alter<T>(in ArrayView<T> parameter);

	public delegate Session<T> Link<T>(in ArrayView<T> parameter);

	public delegate T[] Result<T>(in ArrayView<T> parameter);

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

		readonly IStore<T> _store;
		readonly Result<T> _result;

		public Sessions(IStore<T> store) : this(store, new Copy<T>(store).Get) {}

		public Sessions(IStore<T> store, Result<T> result)
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

	/*public interface ISelect<T> : IStructure<ArrayView<T>, T[]> {}*/

	sealed class LinkedSelector<T> : ISelector<T>
	{
		readonly Link<T>   _previous;
		readonly Result<T> _result;

		public LinkedSelector(Link<T> previous, Result<T> result)
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

	sealed class LinkedView<T> : IResult<T>
	{
		readonly Link<T>   _previous;
		readonly Result<T> _result;

		public LinkedView(Link<T> previous, Result<T> result)
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