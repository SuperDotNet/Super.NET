using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences.Query.Construction;
using System;
using System.Linq.Expressions;

namespace Super.Model.Sequences.Query
{
	public static class Extensions
	{
		public static IContents<TIn, TOut> Returned<TIn, TOut>(this IContents<TIn, TOut> @this)
			=> new ReturnedContents<TIn, TOut>(@this);

		public static IContent<TIn, TOut> Returned<TIn, TOut>(this IContent<TIn, TOut> @this)
			=> new ReturnedContent<TIn, TOut>(@this);
	}

	sealed class Skip : IPartition
	{
		readonly uint _count;

		public Skip(uint count) => _count = count;

		public Selection Get(Selection parameter)
		{
			var count = parameter.Start + _count;
			var result = new Selection(parameter.Length.IsAssigned ? Math.Min(parameter.Length, count) : count,
			                           parameter.Length);
			return result;
		}
	}

	sealed class Take : IPartition
	{
		readonly uint _count;

		public Take(uint count) => _count = count;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start,
			                 parameter.Length.IsAssigned ? Math.Min(parameter.Length, _count) : _count);
	}

	public class Unlimited : LimitAware
	{
		public Unlimited() : base(Assigned<uint>.Unassigned) {}
	}

	public class LimitAware : Instance<Assigned<uint>>, ILimitAware
	{
		public LimitAware(Assigned<uint> instance) : base(instance) {}
	}

	public interface ILimitAware : IResult<Assigned<uint>> {}

	public interface IElement<T> : IElement<T, T> {}

	public interface IElement<TFrom, out TTo> : ISelect<Store<TFrom>, TTo> {}

	public interface IContent<TIn, TOut> : ISelect<Store<TIn>, Store<TOut>> {}

	public interface IBody<T> : IBody<T, T> {}

	public interface IBody<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>> {}

	sealed class Where<T> : IBody<T>
	{
		readonly Func<T, bool>  _where;
		readonly uint           _start;
		readonly Assigned<uint> _until, _limit;

		public Where(Func<T, bool> where, Selection selection, Assigned<uint> limit)
			: this(where, selection.Start, selection.Length, limit) {}

		// ReSharper disable once TooManyDependencies
		public Where(Func<T, bool> where, uint start, Assigned<uint> until, Assigned<uint> limit)
		{
			_where = where;
			_start = start;
			_until = until;
			_limit = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var  to    = parameter.Start + parameter.Length;
			var  array = parameter.Array;
			uint count = 0u, start = 0u;
			var  limit = _limit.Or(_until.Or(parameter.Length));
			for (var i = parameter.Start; i < to && count < limit; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					if (start++ >= _start)
					{
						array[count++] = item;
					}
				}
			}

			return new ArrayView<T>(parameter.Array, 0, count);
		}
	}

	readonly ref struct DynamicStore<T>
	{
		readonly static Leases<T>        Item  = Leases<T>.Default;
		readonly static Leases<Store<T>> Items = Leases<Store<T>>.Default;

		readonly Store<T>[] _stores;
		readonly Selection  _position;
		readonly uint       _index;

		public DynamicStore(uint size, uint length = 32) : this(Items.Get(length).Instance, Selection.Default)
			=> _stores[0] = new Store<T>(Item.Get(size).Instance, 0);

		DynamicStore(Store<T>[] stores, in Selection position, uint index = 0)
		{
			_stores   = stores;
			_position = position;
			_index    = index;
		}

		public Store<T> Get() => Get(DefaultStorage<T>.Default);

		public Store<T> Get(IStores<T> storage)
		{
			var result   = storage.Get(_position.Start + _position.Length);
			var instance = result.Instance;
			using (new Session<Store<T>>(new Store<Store<T>>(_stores), Items))
			{
				var total = _index + 1;
				for (uint i = 0u, offset = 0u; i < total; i++)
				{
					var store = _stores[i];
					using (new Session<T>(store, Item))
					{
						store.Instance.CopyInto(instance, new Selection(0, store.Length), offset);
					}

					offset += store.Length;
				}
			}

			return result;
		}

		public DynamicStore<T> Add(in Store<T> page)
		{
			var current  = _stores[_index];
			var capacity = (uint)current.Instance.Length;
			var filled   = page.Length;
			var size     = filled + current.Length;

			if (size > capacity)
			{
				_stores[_index] =
					new Store<T>(page.Instance.CopyInto(current.Instance, 0, capacity - current.Length, current.Length),
					             capacity);
				var remainder = size - capacity;
				var next      = capacity * 2;
				_stores[_index + 1] =
					new Store<T>(page.Instance
					                 .CopyInto(Item.Get(Math.Max(remainder * 2, Math.Min(int.MaxValue - next, next)))
					                               .Instance,
					                           capacity - current.Length, remainder),
					             remainder);

				return new DynamicStore<T>(_stores, new Selection(_position.Start + capacity, remainder), _index + 1);
			}

			_stores[_index]
				= new Store<T>(page.Instance.CopyInto(current.Instance, 0, filled, current.Length),
				               current.Length + filled);
			return new DynamicStore<T>(_stores, new Selection(_position.Start, _position.Length + filled), _index);
		}
	}

	sealed class Selection<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Func<TIn, TOut> _select;
		readonly Assigned<uint>  _limit;
		readonly IStores<TOut>   _stores;

		// ReSharper disable once TooManyDependencies
		public Selection(IBody<TIn> body, IStores<TOut> stores, Func<TIn, TOut> select, Assigned<uint> limit)
		{
			_body   = body;
			_stores = stores;
			_select = select;
			_limit  = limit;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var body = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length = _limit.IsAssigned
				             ? Math.Min(_limit.Instance, body.Length)
				             : body.Length;
			var result = _stores.Get(length);
			var @in    = body.Array;
			var @out   = result.Instance;

			for (var i = 0; i < length; i++)
			{
				@out[i] = _select(@in[i + body.Start]);
			}

			return result;
		}
	}

	public sealed class InlineProjection<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Copy<TIn, TOut> _apply;
		readonly IStores<TOut>   _stores;
		readonly Assigned<uint>  _limit;

		// ReSharper disable once TooManyDependencies
		public InlineProjection(IBody<TIn> body, Expression<Func<TIn, TOut>> select, IStores<TOut> stores,
		                        Assigned<uint> limit)
			: this(body, InlineSelections<TIn, TOut>.Default.Get(select).Compile(), stores, limit) {}

		// ReSharper disable once TooManyDependencies
		public InlineProjection(IBody<TIn> body, Copy<TIn, TOut> apply,
		                        IStores<TOut> stores, Assigned<uint> limit)
		{
			_body   = body;
			_apply  = apply;
			_stores = stores;
			_limit  = limit;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var body = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));

			var result = _stores.Get(body.Length);

			var bodyStart = body.Start + _limit.Or(body.Length);
			_apply(body.Array, result.Instance, body.Start, bodyStart, 0);

			return result;
		}
	}
}