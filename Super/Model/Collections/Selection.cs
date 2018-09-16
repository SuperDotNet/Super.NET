using Super.Model.Selection;
using System;
using System.Buffers;
using System.Collections;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	/*sealed class Result<TIn, TFrom, TTo> : ISelect<TIn, ArraySegment<TTo>>
	{
		readonly Func<TIn, ArraySegment<TFrom>> _source;
		readonly Func<ArraySegment<TFrom>, ArraySegment<TTo>> _select;

		public Result(Func<TIn, ArraySegment<TFrom>> source, Func<ArraySegment<TFrom>, ArraySegment<TTo>> select)
		{
			_source = source;
			_select = @select;
		}

		public ArraySegment<TTo> Get(TIn parameter) => _select(_source(parameter));
	}*/

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

	public interface ISegment<T> : ISegmentation<T, T> {}

	public interface ISegmentSelect<TIn, TOut> : IEnhancedSelect<Segment<TIn, TOut>, ArraySegment<TOut>> {}

	public interface ISegmentation<TIn, TOut> : IEnhancedSelect<ArraySegment<TIn>, ArraySegment<TOut>> {}

	public interface ISegments<TFrom, TTo> : IEnhancedSelect<ArraySegment<TFrom>, Segment<TFrom, TTo>> {}

	sealed class Segments<TFrom, TTo> : ISegments<TFrom, TTo>
	{
		public static Segments<TFrom, TTo> Default { get; } = new Segments<TFrom, TTo>();

		Segments() : this(ArrayPool<TTo>.Shared) {}

		readonly ArrayPool<TTo> _pool;

		public Segments(ArrayPool<TTo> pool) => _pool = pool;

		public Segment<TFrom, TTo> Get(in ArraySegment<TFrom> parameter)
			=> new Segment<TFrom, TTo>(parameter, _pool.Rent(parameter.Count), _pool);
	}

	public readonly struct Segment<TFrom, TTo> : IDisposable
	{
		readonly ArrayPool<TTo> _pool;

		public Segment(ArraySegment<TFrom> source, TTo[] destination, ArrayPool<TTo> pool)
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

	sealed class Segmentation<TIn, TOut> : ISegmentation<TIn, TOut>
	{
		readonly Selection<ArraySegment<TIn>, Segment<TIn, TOut>>  _segments;
		readonly Selection<Segment<TIn, TOut>, ArraySegment<TOut>> _select;

		public Segmentation(Expression<Func<TIn, TOut>> select) : this(new SegmentSelect<TIn, TOut>(select)) {}

		public Segmentation(ISegmentSelect<TIn, TOut> @select) : this(Segments<TIn, TOut>.Default.Get, select.Get) {}

		public Segmentation(Selection<ArraySegment<TIn>, Segment<TIn, TOut>> segments,
		                    Selection<Segment<TIn, TOut>, ArraySegment<TOut>> select)
		{
			_segments = segments;
			_select   = select;
		}

		public ArraySegment<TOut> Get(in ArraySegment<TIn> parameter)
		{
			using (var context = _segments(in parameter))
			{
				return _select(in context);
			}
		}
	}

	sealed class SegmentSelect<TIn, TOut> : ISegmentSelect<TIn, TOut>
	{
		readonly static ISelect<Expression<Func<TIn, TOut>>, Action<TIn[], TOut[], int, int>>
			Select = InlineSelections<TIn, TOut>.Default.Compile();

		readonly Action<TIn[], TOut[], int, int> _iterate;

		public SegmentSelect(Expression<Func<TIn, TOut>> select) : this(Select.Get(select)) {}

		public SegmentSelect(Action<TIn[], TOut[], int, int> iterate) => _iterate = iterate;

		public ArraySegment<TOut> Get(in Segment<TIn, TOut> parameter)
		{
			_iterate(parameter.Source.Array, parameter.Destination, parameter.Source.Offset, parameter.Source.Count);
			return new ArraySegment<TOut>(parameter.Destination, parameter.Source.Offset, parameter.Source.Count);
		}
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Expression<Func<T, bool>> where) : this(where.Compile()) {}

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArraySegment<T> Get(in ArraySegment<T> parameter)
		{
			var used  = parameter.Count;
			var array = parameter.Array;
			var count = 0;
			for (var i = 0u; i < used; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return new ArraySegment<T>(array, parameter.Offset, count);
		}
	}

	/*sealed class Selection<TFrom, TTo> : ISelection<TFrom, TTo>
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
	}*/

	/*public sealed class WhereSelection<T> : ISelection<T, T>
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
	}*/
}