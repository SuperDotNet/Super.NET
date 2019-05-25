using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences.Query.Construction;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Model.Sequences.Query
{
	public static class Extensions
	{
		/*public static IProjections<TIn, TOut> Returned<TIn, TOut>(this IProjections<TIn, TOut> @this)
			=> new ReturnedProjections<TIn, TOut>(@this);

		*/

		public static IContents<TIn, TOut> Returned<TIn, TOut>(this IContents<TIn, TOut> @this)
			=> new ReturnedContents<TIn, TOut>(@this);

		public static IContent<TIn, TOut> Returned<TIn, TOut>(this IContent<TIn, TOut> @this)
			=> new ReturnedContent<TIn, TOut>(@this);
	}

	/*sealed class Skip<T> : IProject<T>
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
	}*/

	/*sealed class WhereDefinition<T> : Definition<T>
	{
		public WhereDefinition(Func<T, bool> where) : base(Lease<T>.Default, new Where<T>(where)) {}
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

	public interface IContinuation<TFrom, TTo> : ISelect<Store<TFrom>, Store<TTo>> {}*/

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

	static class Build
	{
		public sealed class Select<TIn, TOut> : Builder<TIn, TOut, Func<TIn, TOut>>
		{
			public Select(Func<TIn, TOut> argument)
				: base((shape, stores, parameter, limit)
					       => new Selection<TIn, TOut>(shape, stores, parameter, limit).Returned(),
				       argument) {}
		}

		public sealed class Where<T> : BodyBuilder<T, Func<T, bool>>
		{
			public Where(Func<T, bool> where)
				: base((parameter, selection, limit) => new Query.Where<T>(parameter, selection, limit), where) {}
		}

		public sealed class Distinct<T> : BodyBuilder<T, IEqualityComparer<T>>
		{
			public static Distinct<T> Default { get; } = new Distinct<T>();

			Distinct() : this(EqualityComparer<T>.Default) {}

			public Distinct(IEqualityComparer<T> comparer)
				: base((parameter, selection, limit) => new Query.Distinct<T>(parameter), comparer) {}
		}
	}

	sealed class Where<T> : IBody<T>
	{
		readonly Func<T, bool>  _where;
		readonly uint           _start;
		readonly Assigned<uint> _until, _limit;

		public Where(Func<T, bool> where) : this(where, Selection.Default, Assigned<uint>.Unassigned) {}

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

	/*public class Projections<TIn, TOut> : Builder<TIn, TOut, Func<TIn, TOut>>
	{
		public Projections(Func<TIn, TOut> parameter)
			: base((body, stores, func, limit) => new Projection<TIn, TOut>(func, stores), parameter) {}
	}*/

	public sealed class Concatenations<T> : Builder<T, T, ISequence<T>>
	{
		public Concatenations(ISequence<T> parameter)
			: base((body, stores, sequence, limit) => new Concatenation<T>(sequence, stores), parameter) {}
	}

	public class Concatenation<T> : IContent<T, T>
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
			var length    = parameter.Length;
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

	public sealed class Unions<T> : IContents<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;

		public Unions(ISequence<T> others, IEqualityComparer<T> comparer)
		{
			_others   = others;
			_comparer = comparer;
		}

		public IContent<T, T> Get(Parameter<T, T> parameter) => new Union<T>(_others, _comparer, parameter.Stores);
	}

	public class Union<T> : IContent<T, T>
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
			var length    = parameter.Length;
			var appending = other.Length;

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

	public sealed class Intersections<T> : IContents<T, T>
	{
		readonly ISequence<T>         _others;
		readonly IEqualityComparer<T> _comparer;

		public Intersections(ISequence<T> others, IEqualityComparer<T> comparer)
		{
			_others   = others;
			_comparer = comparer;
		}

		public IContent<T, T> Get(Parameter<T, T> parameter) => new Intersect<T>(_others, _comparer, parameter.Stores);
	}

	public class Intersect<T> : IContent<T, T>
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
			var length    = parameter.Length;
			var appending = other.Length;

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

	/*public class Projection<TFrom, TTo> : IContent<TFrom, TTo>
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
			var length = parameter.Length;
			var result = _stores.Get(length);

			var @out = result.Instance;
			for (var i = 0; i < length; i++)
			{
				@out[i] = _project(@in[i]);
			}

			return result;
		}
	}*/

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

	public class ProjectionManySegment<TIn, TOut> : Builder<TIn, TOut, Func<TIn, IEnumerable<TOut>>>
	{
		public ProjectionManySegment(Func<TIn, IEnumerable<TOut>> parameter)
			: base((body, stores, func, limit) => new ProjectionMany<TIn, TOut>(func, stores), parameter) {}
	}

	public class ProjectionMany<TIn, TOut> : IContent<TIn, TOut>
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
			var length = parameter.Length;
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

	public class InlineProjections<TIn, TOut> : Builder<TIn, TOut, Expression<Func<TIn, TOut>>>
	{
		public InlineProjections(Expression<Func<TIn, TOut>> parameter)
			: base((body, stores, expression, limit) => new InlineProjection<TIn, TOut>(expression, stores), parameter) {}
	}

	public sealed class InlineProjection<TFrom, TTo> : IContent<TFrom, TTo>
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
			var length = parameter.Length;
			var result = _stores.Get(length);

			_apply(parameter.Instance, result.Instance, 0, length);

			return result;
		}
	}
}