using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	public interface ISegment<T> : ISegmentation<T, T> {}

	public interface ISegmentSelect<TIn, TOut> : IEnhancedSelect<Segue<TIn, TOut>, ArrayView<TOut>> {}

	public interface ISegmentation<TIn, TOut> : IEnhancedSelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Segue<TFrom, TTo>
	{
		public Segue(ArrayView<TFrom> source, TTo[] destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ArrayView<TFrom> Source { get; }

		public TTo[] Destination { get; }
	}

	public readonly struct Store<T>
	{
		public Store(T[] instance, uint requested)
		{
			Instance  = instance;
			Requested = requested;
		}

		public T[] Instance { get; }

		public uint Requested { get; }
	}

	/*public interface IStores<T> : ISelect<uint, Store<T>> {}

	sealed class Stores<T> : IStores<T>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : this(EmptyCommand<T[]>.Default.Execute) {}

		readonly Action<T[]> _complete;

		public Stores(Action<T[]> complete) => _complete = complete;

		public Store<T> Get(uint parameter) => new Store<T>(new T[parameter], parameter, _complete);
	}

	sealed class Leased<T> : IStores<T>
	{
		public static Leased<T> Default { get; } = new Leased<T>();

		Leased() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;
		readonly Action<T[]>  _complete;

		public Leased(ArrayPool<T> pool) : this(pool, I<Return<T>>.Default.From(pool).Execute) {}

		public Leased(ArrayPool<T> pool, Action<T[]> complete)
		{
			_pool     = pool;
			_complete = complete;
		}

		public Store<T> Get(uint parameter) => new Store<T>(_pool.Rent((int)parameter), parameter, _complete);
	}*/

	sealed class Return<T> : ICommand<T[]>, IActivateMarker<ArrayPool<T>>
	{
		readonly ArrayPool<T> _pool;

		public Return(ArrayPool<T> pool) => _pool = pool;

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	public readonly struct Selection
	{
		public static Selection Default { get; } = new Selection(0, null);

		public Selection(uint start, uint? length)
		{
			Start  = start;
			Length = length;
		}

		public uint Start { get; }

		public uint? Length { get; }
	}

	/*public interface ISelection<in _, T> : ISelect<_, ArrayView<T>> {}

	readonly struct Node<_, TOut>
	{
		public Node(ISelect<_, ArrayView<TOut>> selector, IStores<TOut> stores) : this(selector, stores, Selection.Default) {}

		public Node(ISelect<_, ArrayView<TOut>> selector, IStores<TOut> stores, Selection selection)
		{
			Selector  = selector;
			Stores    = stores;
			Selection = selection;
		}

		public ISelect<_, ArrayView<TOut>> Selector { get; }

		public Selection Selection { get; }

		public IStores<TOut> Stores { get; }
	}*/

	/*sealed class Stores<T> : ISelect<Selection, ISelect<T[], Store<T>>>
	{
	}*/

	sealed class Storage<T> : DelegatedCommand<T[]>, ISelect<T[], ArrayView<T>>, IActivateMarker<Selection>
	{
		readonly Selection _selection;

		public Storage(Selection selection) : this(selection, EmptyCommand<T[]>.Default.Execute) {}

		public Storage(Selection selection, Action<T[]> complete) : base(complete) => _selection = selection;

		public ArrayView<T> Get(T[] parameter)
			=> new ArrayView<T>(parameter, _selection.Start, _selection.Length ?? (uint)parameter.Length - _selection.Start);
	}

	public interface IStores<T> : ICommand<T[]>, IEnhancedSelect<Selection, ISelect<IEnumerable<T>, ArrayView<T>>> {}

	sealed class ArrayStores<T> : DelegatedCommand<T[]>, IStores<T>
	{
		public static ArrayStores<T> Default { get; } = new ArrayStores<T>();

		ArrayStores() : this(EmptyCommand<T[]>.Default.Execute) {}

		public ArrayStores(Action<T[]> command) : base(command) {}


		public ISelect<IEnumerable<T>, ArrayView<T>> Get(in Selection parameter)
			=> In<IEnumerable<T>>.Select(x => (T[])x)
			                     .Select(new Storage<T>(parameter));
	}

	/*sealed class Fill<T> : ISelect<ICollection<T>, ArrayView<T>>
	{
		public static Fill<T> Default { get; } = new Fill<T>();

		Fill() : this(Lease<T>.Default) {}

		readonly ILease<T> _lease;

		public Fill(ILease<T> lease) => _lease = lease;

		public ArrayView<T> Get(ICollection<T> parameter)
		{
			var result = _lease.Get(parameter.Count);
			parameter.CopyTo(result.Array, 0);
			return result;
		}
	}

	sealed class Iterate<T> : ISelect<IEnumerable<T>, ArrayView<T>>
	{
		public static Iterate<T> Default { get; } = new Iterate<T>();

		Iterate() : this(Enumerate<T>.Default) {}

		readonly IEnumerate<T> _enumerate;

		public Iterate(IEnumerate<T> enumerate) => _enumerate = enumerate;

		public ArrayView<T> Get(IEnumerable<T> parameter) => _enumerate.Get(parameter.GetEnumerator());
	}*/

	public readonly struct ArrayResultView<_, T>
	{
		public ArrayResultView(ISelect<_, IEnumerable<T>> source, IStores<T> stores)
			: this(source, stores, Result<T>.Default, Selection.Default) {}

		// ReSharper disable once TooManyDependencies
		public ArrayResultView(ISelect<_, IEnumerable<T>> source, IStores<T> stores,
		                       IEnhancedSelect<ArrayView<T>, T[]> result, Selection selection)
		{
			Source    = source;
			Stores    = stores;
			Result    = result;
			Selection = selection;
		}

		public ISelect<_, IEnumerable<T>> Source { get; }

		public IEnhancedSelect<ArrayView<T>, T[]> Result { get; }

		public Selection Selection { get; }

		public IStores<T> Stores { get; }
	}

	sealed class ResultSelect<_, T> : IEnhancedSelect<ArrayResultView<_, T>, ISelect<_, T[]>>
	{
		public static ResultSelect<_, T> Default { get; } = new ResultSelect<_, T>();

		ResultSelect() {}

		public ISelect<_, T[]> Get(in ArrayResultView<_, T> parameter)
			=> new ArrayResult<_, T>(parameter.Source.Select(parameter.Stores.Get(parameter.Selection)),
			                         parameter.Result, parameter.Stores.Execute);
	}

	readonly struct Session<T> : IDisposable
	{
		readonly T[]         _parameter;
		readonly Action<T[]> _return;

		public Session(T[] parameter, Action<T[]> @return)
		{
			_parameter = parameter;
			_return    = @return;
		}

		public void Dispose()
		{
			_return?.Invoke(_parameter);
		}
	}

	/*sealed class ArrayView<_, TOut> : ISelect<ISelect<_, TOut[]>, View<_, TOut>>
	{
		public static ArrayView<_,TOut> Default { get; } = new ArrayView<_,TOut>();

		ArrayView() {}

		public View<_, TOut> Get(ISelect<_, TOut[]> parameter)
			=> new View<_, TOut>(parameter.Select(x => new ArrayView<TOut>(x)), Storage<TOut>.Default);
	}*/

	/*public interface ISegue<TIn, TOut> : IEnhancedSelect<Segue<TIn, TOut>, ArrayView<TOut>> {}*/

	/*readonly struct Transition<TFrom, TTo>
	{
		public Transition(ISegue<TFrom, TTo> select, Selection selection)
		{
			Select    = @select;
			Selection = selection;
		}

		public ISegue<TFrom, TTo> Select { get; }
		public Selection Selection { get; }
	}*/

	/*sealed class Selection<_, TFrom, TTo> : ISelection<_, TTo>
	{
		readonly ISelection<_, TFrom>   _previous;
		readonly Transition<TFrom, TTo> _transition;
		readonly IStores<TTo>           _stores;

		public Selection(ISelection<_, TFrom> previous, Transition<TFrom, TTo> transition, IStores<TTo> stores)
		{
			_previous   = previous;
			_transition = transition;
			_stores     = stores;
		}

		public ArrayView<TTo> Get(_ parameter)
		{
			var previous = _previous.Get(parameter)
			                        .Resize(_transition.Selection);

			using (var store = _stores.Get(previous.Count))
			{
				return _transition.Select
				                  .Get(new Segue<TFrom, TTo>(previous, store.Instance));
			}
		}
	}*/

	/*sealed class InitialIteration<_, TFrom, TTo> : ISource<ISelection<_, TTo>>
	{
		public InitialIteration(ISelect<_, IEnumerable<TFrom>> source, Transition<TFrom, TTo> transition)
			: this(source, transition, Stores<TTo>.Default) {}

		public InitialIteration(ISelect<_, IEnumerable<TFrom>> previous, Transition<TFrom, TTo> transition,
		                        IStores<TTo> stores) {}

		public ISelection<_, TTo> Get() => null;
	}*/

	sealed class Segmentation<_, T> : ISegmentation<_, T>
	{
		readonly Selection<Segue<_, T>, ArrayView<T>> _select;
		readonly ILease<_>                                  _source;
		readonly ILease<T>                               _lease;

		public Segmentation(Expression<Func<_, T>> select) : this(new SegmentSelect<_, T>(select)) {}

		public Segmentation(ISegmentSelect<_, T> @select)
			: this(select.Get, Lease<_>.Default, Lease<T>.Default) {}

		public Segmentation(Selection<Segue<_, T>, ArrayView<T>> select, ILease<_> source, ILease<T> lease)
		{
			_select = select;
			_source = source;
			_lease  = lease;
		}

		public ArrayView<T> Get(in ArrayView<_> parameter)
		{
			var lease  = /*_lease.Get(parameter.Count)*/new T[parameter.Count];
			var result = _select(new Segue<_, T>(parameter, lease));
			//_source.Execute(parameter);
			return result;
		}
	}

	sealed class SegmentSelect<TIn, TOut> : ISegmentSelect<TIn, TOut>
	{
		readonly static ISelect<Expression<Func<TIn, TOut>>, Action<TIn[], TOut[], uint, uint>>
			Select = InlineSelections<TIn, TOut>.Default.Compile();

		readonly Action<TIn[], TOut[], uint, uint> _iterate;

		public SegmentSelect(Expression<Func<TIn, TOut>> select) : this(Select.Get(select)) {}

		public SegmentSelect(Action<TIn[], TOut[], uint, uint> iterate) => _iterate = iterate;

		public ArrayView<TOut> Get(in Segue<TIn, TOut> parameter)
		{
			_iterate(parameter.Source.Array, parameter.Destination, parameter.Source.Offset, parameter.Source.Count);
			return new ArrayView<TOut>(parameter.Destination, parameter.Source.Offset, parameter.Source.Count);
		}
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Expression<Func<T, bool>> where) : this(where.Compile()) {}

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Count;
			var array = parameter.Array;
			var count = 0u;
			for (var i = 0u; i < used; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return parameter.Resize(count);
		}
	}

	sealed class SkipSelection<_, T> : IAlteration<ArrayResultView<_, T>>
	{
		readonly uint _skip;

		public SkipSelection(uint skip) => _skip = skip;

		public ArrayResultView<_, T> Get(ArrayResultView<_, T> parameter)
			=> new ArrayResultView<_, T>(parameter.Source, parameter.Stores, parameter.Result,
			                             new Selection(parameter.Selection.Start + _skip, parameter.Selection.Length - _skip));
	}

	/*sealed class SkipSelection<T> : ISegment<T>
	{
		readonly uint _skip;

		public SkipSelection(uint skip) => _skip = skip;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArrayView<T> Get(in ArrayView<T> parameter)
			=> parameter.Resize(parameter.Offset + _skip, parameter.Count - _skip);
	}*/

	sealed class TakeSelection<_, T> : IAlteration<ArrayResultView<_, T>>
	{
		readonly uint _take;

		public TakeSelection(uint take) => _take = take;

		public ArrayResultView<_, T> Get(ArrayResultView<_, T> parameter)
			=> new ArrayResultView<_, T>(parameter.Source, parameter.Stores, parameter.Result,
			                             new Selection(parameter.Selection.Start, _take));
	}
}