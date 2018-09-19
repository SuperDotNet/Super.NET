using Super.Model.Selection;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface ISegment<T> : ISegmentation<T, T> {}

	public interface ISegmentSelect<TIn, TOut> : IEnhancedSelect<Segue<TIn, TOut>, ArrayView<TOut>> {}

	public interface ISegmentation<TIn, TOut> : IEnhancedSelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Segue<TFrom, TTo>
	{
		public Segue(ArrayView<TFrom> source, TTo[] destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ArrayView<TFrom> Source { get; }

		public TTo[] Destination { get; }
	}

	sealed class Segmentation<TIn, TOut> : ISegmentation<TIn, TOut>
	{
		readonly Selection<Segue<TIn, TOut>, ArrayView<TOut>> _select;
		readonly ILease<TIn>                                  _source;
		readonly ILease<TOut>                                 _lease;

		public Segmentation(Expression<Func<TIn, TOut>> select) : this(new SegmentSelect<TIn, TOut>(select)) {}

		public Segmentation(ISegmentSelect<TIn, TOut> @select)
			: this(select.Get, Lease<TIn>.Default, Lease<TOut>.Default) {}

		public Segmentation(Selection<Segue<TIn, TOut>, ArrayView<TOut>> select, ILease<TIn> source, ILease<TOut> lease)
		{
			_select = select;
			_source = source;
			_lease  = lease;
		}

		public ArrayView<TOut> Get(in ArrayView<TIn> parameter)
		{
			var lease  = _lease.Get(parameter.Count);
			var result = _select(new Segue<TIn, TOut>(parameter, lease.Array));
			_source.Execute(parameter);
			return result;
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
			_iterate(view.Array, parameter.Destination, view.Offset, view.Count);
			return new ArrayView<TOut>(parameter.Destination, view.Offset, view.Count);
		}
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Expression<Func<T, bool>> where) : this(where.Compile()) {}

		public WhereSegment(Func<T, bool> where) => _where = where;

		public ArrayView<T> Get(in ArrayView<T> parameter)
		{
			var used  = parameter.Count;
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
			=> parameter.Resize(parameter.Offset + _skip, parameter.Count - _skip);
	}

	sealed class TakeSelection<T> : ISegment<T>
	{
		readonly uint _take;

		public TakeSelection(uint take) => _take = take;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArrayView<T> Get(in ArrayView<T> parameter) => parameter.Resize(_take);
	}
}