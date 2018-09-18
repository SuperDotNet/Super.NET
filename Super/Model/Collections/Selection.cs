using Super.Model.Selection;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface ISegment<T> : ISegmentation<T, T> {}

	public interface ISegmentSelect<TIn, TOut> : IEnhancedSelect<Segue<TIn, TOut>, ArrayView<TOut>> {}

	public interface ISegmentation<TIn, TOut> : IEnhancedSelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public interface ISegments<TFrom, TTo> : IEnhancedSelect<ArrayView<TFrom>, Segue<TFrom, TTo>> {}

	sealed class Segments<TFrom, TTo> : ISegments<TFrom, TTo>
	{
		public static Segments<TFrom, TTo> Default { get; } = new Segments<TFrom, TTo>();

		Segments() : this(Lease<TTo>.Default) {}

		readonly ILease<TTo> _lease;

		public Segments(ILease<TTo> lease) => _lease = lease;

		public Segue<TFrom, TTo> Get(in ArrayView<TFrom> parameter)
			=> new Segue<TFrom, TTo>(parameter, _lease.Get(parameter.Length));
	}

	public readonly struct Segue<TFrom, TTo> : IDisposable
	{
		public Segue(ArrayView<TFrom> source, ArrayView<TTo> destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ArrayView<TFrom> Source { get; }

		public ArrayView<TTo> Destination { get; }

		public void Dispose()
		{
			Source.Dispose();
			Destination.Dispose();
		}
	}

	/*sealed class Segue<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>>
	{
		readonly ISegmentation<TIn, TOut> _segmentation;

		public Segue(ISegmentation<TIn, TOut> segmentation) => _segmentation = segmentation;

		public ArrayView<TOut> Get(ArrayView<TIn> parameter)
		{
			using (parameter)
			{
				return _segmentation.Get(parameter);
			}
		}
	}*/

	sealed class Segmentation<TIn, TOut> : ISegmentation<TIn, TOut>
	{
		readonly Selection<ArrayView<TIn>, Segue<TIn, TOut>>  _segments;
		readonly Selection<Segue<TIn, TOut>, ArrayView<TOut>> _select;

		public Segmentation(Expression<Func<TIn, TOut>> select) : this(new SegmentSelect<TIn, TOut>(select)) {}

		public Segmentation(ISegmentSelect<TIn, TOut> @select) : this(Segments<TIn, TOut>.Default.Get, select.Get) {}

		public Segmentation(Selection<ArrayView<TIn>, Segue<TIn, TOut>> segments,
		                    Selection<Segue<TIn, TOut>, ArrayView<TOut>> select)
		{
			_segments = segments;
			_select   = select;
		}

		public ArrayView<TOut> Get(in ArrayView<TIn> parameter)
		{
			using (var context = _segments(in parameter))
			{
				return _select(in context);
			}
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
			var view = parameter.Source;
			_iterate(view.Array, parameter.Destination.Array, view.Start, view.Length);
			return parameter.Destination;
		}
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Expression<Func<T, bool>> where) : this(where.Compile()) {}

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Length;
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

	sealed class SkipSelection<T> : ISegment<T>
	{
		readonly uint _skip;

		public SkipSelection(uint skip) => _skip = skip;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArrayView<T> Get(in ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Array, parameter.Start + _skip, parameter.Length - _skip);
	}

	sealed class TakeSelection<T> : ISegment<T>
	{
		readonly uint _take;

		public TakeSelection(uint take) => _take = take;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArrayView<T> Get(in ArrayView<T> parameter) => new ArrayView<T>(parameter.Array, parameter.Start, _take);
	}
}