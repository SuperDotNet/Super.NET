using Super.Model.Selection;
using Super.Model.Sequences.Query.Construction;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Model.Sequences.Query
{
	public static class Extensions
	{
		public static IProjections<TIn, TOut> Returned<TIn, TOut>(this IProjections<TIn, TOut> @this)
			=> new ReturnedProjections<TIn, TOut>(@this);

		public static IContinuation<TIn, TOut> Returned<TIn, TOut>(this IContinuation<TIn, TOut> @this)
			=> new ReturnedContinuation<TIn, TOut>(@this);
	}

	sealed class Skip<T> : IProject<T>
	{
		readonly uint _count;

		public Skip(uint count) => _count = count;

		public ArrayView<T> Get(ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Array,
			                    Math.Min(parameter.Length, parameter.Start + _count),
			                    Math.Max(parameter.Start, (parameter.Length - (int)_count).Clamp0()));
	}

	sealed class Take<T> : IProject<T>
	{
		readonly uint _count;

		public Take(uint count) => _count = count;

		public ArrayView<T> Get(ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Array, parameter.Start, Math.Min(parameter.Length, _count));
	}

	sealed class WhereDefinition<T> : Definition<T>
	{
		public WhereDefinition(Func<T, bool> where) : base(Leased<T>.Default, new Where<T>(where)) {}
	}

	public class Where<T> : IProject<T>
	{
		readonly Func<T, bool> _where;

		public Where(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var to    = parameter.Start + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			for (var i = parameter.Start; i < to; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return new ArrayView<T>(parameter.Array, 0, count);
		}
	}

	public interface IContinuation<TFrom, TTo> : ISelect<Store<TFrom>, Store<TTo>> {}

	public class Projections<TIn, TOut> : Continuations<Func<TIn, TOut>, TIn, TOut>
	{
		public Projections(Func<TIn, TOut> parameter)
			: base((x, stores) => new Projection<TIn, TOut>(x, stores), parameter) {}
	}

	public sealed class Concatenations<T> : Continuations<ISequence<T>, T, T>
	{
		public Concatenations(ISequence<T> parameter)
			: base((sequence, stores) => new Concatenation<T>(sequence, stores), parameter) {}
	}

	public class Concatenation<T> : IContinuation<T, T>
	{
		readonly ISequence<T> _others;
		readonly IStores<T>   _stores;

		public Concatenation(ISequence<T> others, IStores<T> stores)
		{
			_others = others;
			_stores = stores;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var other     = _others.Get();
			var source    = other.Instance;
			var @in       = parameter.Instance;
			var length    = parameter.Length.Or((uint)@in.Length);
			var appending = other.Length;
			var total     = length + appending;
			var result    = _stores.Get(total);

			var @out = @in.CopyInto(result.Instance);
			for (var i = 0; i < appending; i++)
			{
				@out[i + length] = source[i];
			}

			return result;
		}
	}

	public sealed class Unions<T> : IProjections<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;

		public Unions(ISequence<T> others, IEqualityComparer<T> comparer)
		{
			_others   = others;
			_comparer = comparer;
		}

		public IContinuation<T, T> Get(IStores<T> parameter) => new Union<T>(_others, _comparer, parameter);
	}

	public class Union<T> : IContinuation<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;
		readonly IStores<T>           _stores;

		public Union(ISequence<T> others, IEqualityComparer<T> comparer, IStores<T> stores)
		{
			_others   = others;
			_comparer = comparer;
			_stores   = stores;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var other     = _others.Get();
			var source    = other.Instance;
			var @in       = parameter.Instance;
			var length    = parameter.Length.Or((uint)@in.Length);
			var appending = other.Length.Or((uint)source.Length);

			var set = new Set<T>(_comparer);
			for (var i = 0u; i < length; i++)
			{
				set.Add(in @in[i]);
			}

			var count = 0u;
			for (var i = 0u; i < appending; i++)
			{
				var item = source[i];
				if (set.Add(in item))
				{
					source[count++] = item;
				}
			}

			var result = _stores.Get(length + count);

			var @out = @in.CopyInto(result.Instance);
			for (var i = 0; i < count; i++)
			{
				@out[i + length] = source[i];
			}

			return result;
		}
	}

	public sealed class Intersections<T> : IProjections<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;

		public Intersections(ISequence<T> others, IEqualityComparer<T> comparer)
		{
			_others   = others;
			_comparer = comparer;
		}

		public IContinuation<T, T> Get(IStores<T> parameter) => new Intersect<T>(_others, _comparer, parameter);
	}

	public class Intersect<T> : IContinuation<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;
		readonly IStores<T>           _stores;

		public Intersect(ISequence<T> others, IEqualityComparer<T> comparer, IStores<T> stores)
		{
			_others   = others;
			_comparer = comparer;
			_stores   = stores;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var other     = _others.Get();
			var source    = other.Instance;
			var @in       = parameter.Instance;
			var length    = parameter.Length.Or((uint)@in.Length);
			var appending = other.Length.Or((uint)source.Length);

			var set = new Set<T>(_comparer);
			for (var i = 0u; i < length; i++)
			{
				set.Add(in @in[i]);
			}

			var count = 0u;
			for (var i = 0u; i < appending; i++)
			{
				var item = source[i];
				if (!set.Add(in item))
				{
					source[count++] = item;
				}
			}

			var result = _stores.Get(count);

			source.CopyInto(result.Instance, 0, count);

			return result;
		}
	}

	public class Projection<TFrom, TTo> : IContinuation<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _project;
		readonly IStores<TTo>     _stores;

		public Projection(Func<TFrom, TTo> project, IStores<TTo> stores)
		{
			_project = project;
			_stores  = stores;
		}

		public Store<TTo> Get(Store<TFrom> parameter)
		{
			var @in    = parameter.Instance;
			var length = parameter.Length();
			var result = _stores.Get(length);

			var @out = result.Instance;
			for (var i = 0; i < length; i++)
			{
				@out[i] = _project(@in[i]);
			}

			return result;
		}
	}

	public class ProjectionManySegment<TIn, TOut> : Continuations<Func<TIn, IEnumerable<TOut>>, TIn, TOut>
	{
		public ProjectionManySegment(Func<TIn, IEnumerable<TOut>> parameter)
			: base((x, stores) => new ProjectionMany<TIn, TOut>(x, stores), parameter) {}
	}

	public class ProjectionMany<TIn, TOut> : IContinuation<TIn, TOut>
	{
		readonly Func<TIn, IEnumerable<TOut>> _project;
		readonly IStores<TOut>                _stores;
		readonly IIterate<TOut>               _iterate;

		public ProjectionMany(Func<TIn, IEnumerable<TOut>> project, IStores<TOut> stores)
			: this(project, stores, Iterate<TOut>.Default) {}

		public ProjectionMany(Func<TIn, IEnumerable<TOut>> project, IStores<TOut> stores, IIterate<TOut> iterate)
		{
			_project = project;
			_stores  = stores;
			_iterate = iterate;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var @in    = parameter.Instance;
			var length = parameter.Length.Or((uint)@in.Length);
			var store  = new DynamicStore<TOut>(1024);
			for (var i = 0; i < length; i++)
			{
				var enumerable = _project(@in[i]);
				var page       = _iterate.Get(enumerable);
				store = store.Add(in page);
			}

			var result = store.Get(_stores);
			return result;
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

		DynamicStore(Store<T>[] stores, Selection position, uint index = 0)
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
			var stores   = _stores;
			var current  = stores[_index];
			var capacity = (uint)current.Instance.Length;
			var max      = capacity;
			var filled   = page.Length.Or((uint)page.Instance.Length);
			var size     = filled + current.Length;

			if (size > max)
			{
				stores[_index] =
					new Store<T>(page.Instance.CopyInto(current.Instance, 0, capacity - current.Length, current.Length),
					             capacity);
				var remainder = size - max;
				var next      = capacity * 2;
				stores[_index + 1] =
					new
						Store<T>(page.Instance
						             .CopyInto(Item.Get(Math.Max(remainder * 2, Math.Min(int.MaxValue - next, next)))
						                           .Instance,
						                       capacity - current.Length, remainder),
						         remainder);

				return new DynamicStore<T>(_stores, new Selection(_position.Start + max, remainder), _index + 1);
			}

			stores[_index]
				= new
					Store<T>(page.Instance.CopyInto(current.Instance, 0, filled, current.Length),
					         current.Length + filled);
			return new DynamicStore<T>(_stores, new Selection(_position.Start, _position.Length + filled), _index);
		}
	}

	public class InlineProjections<TIn, TOut> : Continuations<Expression<Func<TIn, TOut>>, TIn, TOut>
	{
		public InlineProjections(Expression<Func<TIn, TOut>> parameter)
			: base((expression, stores) => new InlineProjection<TIn, TOut>(expression, stores), parameter) {}
	}

	public sealed class InlineProjection<TFrom, TTo> : IContinuation<TFrom, TTo>
	{
		readonly Action<TFrom[], TTo[], uint, uint> _apply;
		readonly IStores<TTo>                       _stores;

		public InlineProjection(Expression<Func<TFrom, TTo>> select, IStores<TTo> stores)
			: this(InlineSelections<TFrom, TTo>.Default.Get(select).Compile(), stores) {}

		public InlineProjection(Action<TFrom[], TTo[], uint, uint> apply, IStores<TTo> stores)
		{
			_apply  = apply;
			_stores = stores;
		}

		public Store<TTo> Get(Store<TFrom> parameter)
		{
			var @in    = parameter.Instance;
			var length = parameter.Length.Or((uint)@in.Length);
			var result = _stores.Get(length);

			_apply(parameter.Instance, result.Instance, 0, length);

			return result;
		}
	}
}