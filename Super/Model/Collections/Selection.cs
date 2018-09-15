using Super.Model.Selection;
using System;
using System.Buffers;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	sealed class Result<TIn, TFrom, TTo> : ISelect<TIn, ArraySegment<TTo>>
	{
		readonly ISelect<TIn, ArraySegment<TFrom>> _source;
		readonly IViewSelect<TFrom, TTo>           _select;

		public Result(ISelect<TIn, ArraySegment<TFrom>> source, IViewSelect<TFrom, TTo> select)
		{
			_source = source;
			_select = @select;
		}

		public ArraySegment<TTo> Get(TIn parameter) => _select.Get(_source.Get(parameter));
	}

	public interface ILoader<T> : ISelect<IEnumerable, ArraySegment<T>> {}

	sealed class Loader<T> : ILoader<T>
	{
		public static Loader<T> Default { get; } = new Loader<T>();

		Loader() {}

		public ArraySegment<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return new ArraySegment<T>(array);
			}

			throw new InvalidOperationException($"Unsupported view type: {parameter.GetType().FullName}");
		}
	}

	public interface IViewSelect<TIn, TOut> : IEnhancedSelect<ArraySegment<TIn>, ArraySegment<TOut>> {}

	public interface IArrays<TFrom, TTo> : IEnhancedSelect<ArraySegment<TFrom>, ArrayContext<TFrom, TTo>> {}

	sealed class Arrays<TFrom, TTo> : IArrays<TFrom, TTo>
	{
		public static Arrays<TFrom, TTo> Default { get; } = new Arrays<TFrom, TTo>();

		Arrays() : this(ArrayPool<TTo>.Shared) {}

		readonly ArrayPool<TTo> _pool;

		public Arrays(ArrayPool<TTo> pool) => _pool = pool;

		public ArrayContext<TFrom, TTo> Get(in ArraySegment<TFrom> parameter)
			=> new ArrayContext<TFrom, TTo>(parameter, _pool.Rent(parameter.Count), _pool);
	}

	public readonly struct ArrayContext<TFrom, TTo> : IDisposable
	{
		readonly ArrayPool<TTo> _pool;

		public ArrayContext(ArraySegment<TFrom> source, TTo[] destination, ArrayPool<TTo> pool)
		{
			_pool       = pool;
			Source      = source;
			Destination = destination;
		}

		public ArraySegment<TFrom> Source { get; }

		public TTo[] Destination { get; }

		public void Dispose()
		{
			_pool.Return(Destination);
		}
	}

	sealed class ViewSelector<TIn, TOut> : IViewSelect<TIn, TOut>
	{
		readonly IArrays<TIn, TOut> _arrays;
		readonly IViewSelection<TIn, TOut>                        _selection;

		public ViewSelector(IViewSelection<TIn, TOut> selection) : this(Arrays<TIn, TOut>.Default, selection) {}

		public ViewSelector(IArrays<TIn, TOut> arrays, IViewSelection<TIn, TOut> selection)
		{
			_arrays    = arrays;
			_selection = selection;
		}

		public ArraySegment<TOut> Get(in ArraySegment<TIn> parameter)
		{
			using (var context = _arrays.Get(in parameter))
			{
				return _selection.Get(context);
			}
		}
	}

	public interface IViewSelection<TIn, TOut> : IEnhancedSelect<ArrayContext<TIn, TOut>, ArraySegment<TOut>> {}

	sealed class ViewSelect<TIn, TOut> : IViewSelection<TIn, TOut>
	{
		readonly static ISelect<Expression<Func<TIn, TOut>>, Action<TIn[], TOut[], int, int>>
			Select = InlineSelections<TIn, TOut>.Default.Compile();

		readonly Action<TIn[], TOut[], int, int> _iterate;

		public ViewSelect(Expression<Func<TIn, TOut>> select) : this(Select.Get(select)) {}

		public ViewSelect(Action<TIn[], TOut[], int, int> iterate) => _iterate = iterate;

		public ArraySegment<TOut> Get(in ArrayContext<TIn, TOut> parameter)
		{
			var destination = parameter.Destination;
			var source      = parameter.Source;
			var used        = source.Count;
			_iterate(source.Array, destination, source.Offset, used);
			return new ArraySegment<TOut>(destination, source.Offset, used);
		}
	}

	sealed class WhereView<T> : IViewSelection<T, T>
	{
		readonly Func<T, bool> _where;

		public WhereView(Expression<Func<T, bool>> where) : this(where.Compile()) {}

		public WhereView(Func<T, bool> where) => _where = @where;

		public ArraySegment<T> Get(in ArrayContext<T, T> parameter)
		{
			var used        = parameter.Source.Count;
			var count       = 0;
			for (var i = 0u; i < used; i++)
			{
				var item = parameter.Source.Array[i];
				if (_where(item))
				{
					parameter.Destination[count++] = item;
				}
			}

			return new ArraySegment<T>(parameter.Destination, parameter.Source.Offset, count);
		}
	}

	sealed class Selection<TFrom, TTo> : ISelection<TFrom, TTo>
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