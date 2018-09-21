using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
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

	sealed class Result<T> : IEnhancedSelect<ArrayView<T>, T[]>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() {}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in ArrayView<T> parameter)
			=> parameter.Start == 0 && parameter.Length == parameter.Array.Length ? parameter.Array : Fill(parameter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static T[] Fill(in ArrayView<T> parameter)
		{
			var length = (int)parameter.Length;
			var result = new T[length];
			Array.ConstrainedCopy(parameter.Array, (int)parameter.Start, result, 0, length);
			return result;
		}
	}

	sealed class Views<T> : ISelect<T[], ArrayView<T>>
	{
		readonly Selection _selection;

		public Views(Selection selection) => _selection = selection;

		public ArrayView<T> Get(T[] parameter)
			=> new ArrayView<T>(parameter, _selection.Start,
			                    _selection.Length ?? (uint)parameter.Length - _selection.Start);
	}

	public interface IStores<T> : ISource<Complete<T>>,
	                              IEnhancedSelect<Selection, ISelect<IEnumerable<T>, ArrayView<T>>> {}

	sealed class ArrayStores<T> : Source<Complete<T>>, IStores<T>
	{
		public static ArrayStores<T> Default { get; } = new ArrayStores<T>();

		ArrayStores() : base(null) {}

		public ISelect<IEnumerable<T>, ArrayView<T>> Get(in Selection parameter)
			=> In<IEnumerable<T>>.Select(x => (T[])x)
			                     .Select(new Views<T>(parameter));
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

	sealed class ResultSelect<_, T> : IEnhancedSelect<ArrayResultView<_, T>, ISelect<_, T[]>>
	{
		public static ResultSelect<_, T> Default { get; } = new ResultSelect<_, T>();

		ResultSelect() {}

		public ISelect<_, T[]> Get(in ArrayResultView<_, T> parameter)
		{
			var complete = parameter.Stores.Get();
			var @select  = parameter.Source.Select(parameter.Stores.Get(parameter.Selection));
			var result = complete != null
				             ? new Returned<_, T>(select, parameter.Result, complete)
				             : @select.Select(parameter.Result);
			return result;
		}
	}

	public delegate void Complete<in T>(T[] resource);

	sealed class Returned<_, T> : ISelect<_, T[]>
	{
		readonly ISelect<_, ArrayView<T>>           _previous;
		readonly IEnhancedSelect<ArrayView<T>, T[]> _next;
		readonly Complete<T>                        _return;

		public Returned(ISelect<_, ArrayView<T>> previous, IEnhancedSelect<ArrayView<T>, T[]> next, Complete<T> @return)
		{
			_previous = previous;
			_next     = next;
			_return   = @return;
		}

		public T[] Get(_ parameter)
		{
			var view = _previous.Get(parameter);
			using (new Session<T>(view.Array, _return))
			{
				return _next.Get(in view);
			}
		}
	}

	readonly struct Session<T> : IDisposable
	{
		readonly T[]         _parameter;
		readonly Complete<T> _return;

		public Session(T[] parameter, Complete<T> @return)
		{
			_parameter = parameter;
			_return    = @return;
		}

		// [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_return.Invoke(_parameter);
		}
	}

	public interface ILink<_, T> : IEnhancedSelect<ArrayResultView<_, T>, ArrayResultView<_, T>> {}

	sealed class SkipLink<_, T> : ILink<_, T>
	{
		readonly uint _skip;

		public SkipLink(uint skip) => _skip = skip;

		public ArrayResultView<_, T> Get(in ArrayResultView<_, T> parameter)
			=> new ArrayResultView<_, T>(parameter.Source, parameter.Stores, parameter.Result,
			                             new Selection(parameter.Selection.Start + _skip,
			                                           parameter.Selection.Length - _skip));
	}

	sealed class TakeLink<_, T> : ILink<_, T>
	{
		readonly uint _take;

		public TakeLink(uint take) => _take = take;

		public ArrayResultView<_, T> Get(in ArrayResultView<_, T> parameter)
			=> new ArrayResultView<_, T>(parameter.Source, parameter.Stores, parameter.Result,
			                             new Selection(parameter.Selection.Start, _take));
	}

	sealed class WhereLink<_, T> : LinkAlteration<_, T>
	{
		public WhereLink(Func<T, bool> where) : base(new WhereSegment<T>(where)) {}
	}

	class LinkAlteration<_, T> : ILink<_, T>
	{
		readonly IStoreAlteration<T> _alteration;

		public LinkAlteration(ISegment<T> segment) : this(new StoreAlteration<T>(segment)) {}

		public LinkAlteration(IStoreAlteration<T> alteration) => _alteration = alteration;

		public ArrayResultView<_, T> Get(in ArrayResultView<_, T> parameter)
			=> new ArrayResultView<_, T>(parameter.Source,
			                             new StoresAlteration<T>(parameter.Stores, _alteration),
			                             parameter.Result, parameter.Selection);
	}

	public interface IStoreAlteration<T> : IAlteration<ISelect<IEnumerable<T>, ArrayView<T>>> {}

	sealed class StoresAlteration<T> : IStores<T>
	{
		readonly IStores<T> _stores;
		readonly IStoreAlteration<T> _alteration;

		public StoresAlteration(IStores<T> stores, IStoreAlteration<T> alteration)
		{
			_stores = stores;
			_alteration = alteration;
		}

		public Complete<T> Get() => _stores.Get();

		public ISelect<IEnumerable<T>, ArrayView<T>> Get(in Selection parameter)
			=> _alteration.Get(_stores.Get(in parameter));
	}

	sealed class StoreAlteration<T> : IStoreAlteration<T>
	{
		readonly ISegment<T> _select;

		public StoreAlteration(ISegment<T> select) => _select = @select;

		public ISelect<IEnumerable<T>, ArrayView<T>> Get(ISelect<IEnumerable<T>, ArrayView<T>> parameter)
			=> parameter.Select(_select);
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = parameter.Start; i < used; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return parameter.Resize(0, count);
		}
	}
}