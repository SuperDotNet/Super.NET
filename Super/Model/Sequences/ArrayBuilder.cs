using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface IState<T> : IAlteration<T> {}

	public interface IBuilder<TIn, T> : ISource<ISelector<TIn, T>>,
	                                    ISelect<IState<Collections.Selection>, IBuilder<TIn, T>>,
	                                    ISelect<ISegment<T>, IBuilder<TIn, T>> {}

	public interface IBuilder<T> : IBuilder<T[], T> {}

	sealed class ArrayBuilder<T> : FixedSelection<IStore<T>, ISelector<T[], T>>, IBuilder<T>
	{
		public static ArrayBuilder<T> Default { get; } = new ArrayBuilder<T>();

		ArrayBuilder() : this(ArraySelectors<T>.Default) {}

		readonly IArraySelectors<T> _selectors;

		public ArrayBuilder(IArraySelectors<T> selectors) : base(selectors, Allocated<T>.Default)
			=> _selectors = selectors;

		public IBuilder<T[], T> Get(IState<Collections.Selection> parameter)
			=> new ArrayBuilder<T>(_selectors.Get(parameter));

		public IBuilder<T[], T> Get(ISegment<T> parameter)
			=> new ArrayBuilder<T>(new SegmentedArraySelectors<T>(_selectors, parameter));
	}

	public interface IArraySelectors<T> : ISelect<IState<Collections.Selection>, IArraySelectors<T>>,
	                                      ISelect<IStore<T>, ISelector<T>>,
	                                      ISource<ISegment<T>> {}

	sealed class SegmentedArraySelectors<T> : IArraySelectors<T>
	{
		readonly Link<T>            _previous;
		readonly ISegment<T>        _segment;
		readonly IArraySelectors<T> _selectors;

		public SegmentedArraySelectors(IArraySelectors<T> selectors, ISegment<T> segment)
			: this(selectors.Get().Select(Sessions<T>.Default).Get, segment, selectors) {}

		public SegmentedArraySelectors(Link<T> previous, ISegment<T> segment, IArraySelectors<T> selectors)
		{
			_previous  = previous;
			_segment   = segment;
			_selectors = selectors;
		}

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter)
			=> new SegmentedArraySelectors<T>(_previous, _segment, _selectors.Get(parameter));

		public ISelector<T> Get(IStore<T> parameter)
			=> new LinkedSelector<T>(_previous, _segment.Select(_selectors.Get())
			                                            .Select(new Copy<T>(parameter))
			                                            .Get);

		public ISegment<T> Get() => new LinkedSegment<T>(_previous, _selectors.Current(_segment));
	}

	sealed class ArraySelectors<T> : Source<ISegment<T>>, IArraySelectors<T>
	{
		public static ArraySelectors<T> Default { get; } = new ArraySelectors<T>();

		ArraySelectors() : this(Collections.Selection.Default) {}

		readonly Collections.Selection _selection;

		public ArraySelectors(Collections.Selection selection) : base(new SelectedSegment<T>(selection))
			=> _selection = selection;

		public IArraySelectors<T> Get(IState<Collections.Selection> parameter)
			=> new ArraySelectors<T>(parameter.Get(_selection));

		public ISelector<T> Get(IStore<T> parameter) => new Selector<T>(parameter, _selection);
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

	/*sealed class SegmentSelector<T> : ISelect<T>
	{
		readonly Alter<T>                  _alter;
		readonly Result<ArrayView<T>, T[]> _select;

		public SegmentSelector(Alter<T> alter, Result<ArrayView<T>, T[]> select)
		{
			_alter  = alter;
			_select = @select;
		}

		public T[] Get(in ArrayView<T> parameter) => _select(_alter(in parameter));
	}*/

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

	sealed class Copy<T> : IStructure<ArrayView<T>, T[]>
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
				var selection = new ArrayView<T>(parameter, 0, session.Length);
				return _result(in selection);
			}
		}
	}
}