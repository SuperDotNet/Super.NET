using Super.Model.Selection;
using System;
using System.Buffers;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
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

	sealed class SkipSelection<T> : ISegment<T>
	{
		readonly int _skip;

		public SkipSelection(int skip) => _skip = skip;

		public ArraySegment<T> Get(in ArraySegment<T> parameter)
			=> new ArraySegment<T>(parameter.Array, parameter.Offset + _skip, parameter.Count - _skip);
	}

	sealed class TakeSelection<T> : ISegment<T>
	{
		readonly int _take;

		public TakeSelection(int take) => _take = take;

		public ArraySegment<T> Get(in ArraySegment<T> parameter)
			=> new ArraySegment<T>(parameter.Array, parameter.Offset, _take);
	}
}