using Super.Model.Selection;
using System;
using System.Buffers;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	public sealed class Selection<TFrom, TTo> : ISelection<TFrom, TTo>
	{
		readonly static ArrayPool<TTo> Pool = ArrayPool<TTo>.Shared;

		readonly static ISelect<Expression<Func<TFrom, TTo>>, Action<TFrom[], TTo[], int, int>> Select
			= InlineSelections<TFrom, TTo>.Default.Compile();

		readonly Action<TFrom[], TTo[], int, int> _iterate;
		readonly ArrayPool<TTo>                   _pool;

		public Selection(Expression<Func<TFrom, TTo>> select) : this(Select.Get(select), Pool) {}

		public Selection(Action<TFrom[], TTo[], int, int> iterate, ArrayPool<TTo> pool)
		{
			_iterate = iterate;
			_pool    = pool;
		}

		public View<TTo> Get(View<TFrom> parameter)
		{
			ref var view = ref parameter;

			var used = (int)view.Used;

			var destination = _pool.Rent(used);

			_iterate(view.Source, destination, 0, used);

			view.Release();

			return new View<TTo>(_pool, new ArraySegment<TTo>(destination, 0, used));
		}
	}

	public sealed class WhereSelection<T> : ISelection<T, T>
	{
		readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

		readonly Func<T, bool> _where;
		readonly ArrayPool<T>  _pool;

		public WhereSelection(Func<T, bool> where) : this(@where, Pool) {}

		public WhereSelection(Func<T, bool> where, ArrayPool<T> pool)
		{
			_where = @where;
			_pool  = pool;
		}

		public View<T> Get(View<T> parameter)
		{
			var view        = parameter;
			var used        = (int)view.Used;
			var destination = _pool.Rent(used);
			var count       = 0;
			var source      = view.Source;
			for (var i = 0u; i < used; i++)
			{
				var item = source[i];
				if (_where(item))
				{
					destination[count++] = item;
				}
			}

			view.Release();

			return new View<T>(_pool, new ArraySegment<T>(destination, 0, count));
		}
	}
}