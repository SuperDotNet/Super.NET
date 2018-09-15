using Super.Model.Selection;
using System;
using System.Buffers;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	sealed class SelectionWhere<TFrom, TTo> : ISelection<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;
		readonly ArrayPool<TTo>   _pool;

		public SelectionWhere(Expression<Func<TFrom, TTo>> select, Expression<Func<TTo, bool>> where)
			: this(select.Compile(), where.Compile(), ArrayPool<TTo>.Shared) {}

		public SelectionWhere(Func<TFrom, TTo> select, Func<TTo, bool> where, ArrayPool<TTo> pool)
		{
			_select = @select;
			_where  = @where;
			_pool   = pool;
		}

		public View<TTo> Get(View<TFrom> parameter)
		{
			ref var view        = ref parameter;
			var     used        = (int)view.Used;
			var     result      = view.Select(_pool);
			var     source      = view.Source;
			var     destination = result.Source;

			var count = 0u;
			for (var i = 0u; i < used; i++)
			{
				var item = _select(source[i]);
				if (_where(item))
				{
					destination[count++] = item;
				}
			}

			view.Release();

			return result.Resize(count);
		}
	}

	/*sealed class ExpressionSelection<TFrom, TTo> : ISelection<TFrom, TTo>
	{
		readonly static ArrayPool<TTo> Pool = ArrayPool<TTo>.Shared;

		readonly static ISelect<Expression<Func<TFrom, TTo>>, Action<TFrom[], TTo[], int, int>> Select
			= InlineSelections<TFrom, TTo>.Default.Compile();

		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ExpressionSelection(Expression<Func<TFrom, TTo>> select) : this(Select.Get(select)) {}

		public ExpressionSelection(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public View<TTo> Get(View<TFrom> parameter)
		{
			var length = (int)parameter.Used;
			var store = Pool.Rent(length);
			var result = new View<TTo>(store, store.AsMemory(length), Pool);

			_iterate(parameter.Peek(), store, 0, length);
			parameter.Release();

			return result;
		}
	}

	public sealed class ExpressionSelector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly static ISelect<Expression<Func<TFrom, TTo>>, Action<TFrom[], TTo[], int, int>> Select
			= InlineSelections<TFrom, TTo>.Default.Compile();

		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ExpressionSelector(Expression<Func<TFrom, TTo>> select) : this(Select.Get(select)) {}

		public ExpressionSelector(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result  = new TTo[length];

			_iterate(parameter.Segment()?.Array, result, 0, length);

			return result;
		}
	}*/
}