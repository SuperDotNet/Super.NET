using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class ArraySelector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelector(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelector(Func<TFrom, TTo> select) => _select = select;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];
			var span   = parameter.Span;

			for (var i = 0; i < length; i++)
			{
				result[i] = _select(span[i]);
			}

			return result;
		}
	}

	public class Where<TIn, TOut> : ISelect<TIn, ReadOnlyMemory<TOut>>
	{
		readonly ISelect<TIn, ReadOnlyMemory<TOut>> _select;
		readonly Func<TOut, bool>                   _where;

		public Where(ISelect<TIn, ReadOnlyMemory<TOut>> select, ISpecification<TOut> where)
			: this(select, where.IsSatisfiedBy) {}

		public Where(ISelect<TIn, ReadOnlyMemory<TOut>> select, Func<TOut, bool> where)
		{
			_select = @select;
			_where  = @where;
		}

		public ReadOnlyMemory<TOut> Get(TIn parameter)
		{
			var items  = _select.Get(parameter);
			var length = items.Length;

			Span<int> indexes = stackalloc int[length];
			var       count   = 0;
			var       source  = items.Span;
			for (var i = 0; i < length; i++)
			{
				var element = source[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new TOut[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = source[indexes[i]];
			}

			//ArrayPool<TOut>.Shared.Return(items.Array());

			return result;
		}
	}

	public class Where<T> : ISelector<T, T>
	{
		readonly Func<T, bool> _where;

		public Where(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public Where(Func<T, bool> where) => _where = where;

		public ReadOnlyMemory<T> Get(ReadOnlyMemory<T> parameter)
		{
			var length = parameter.Length;

			Span<int> indexes = stackalloc int[length];
			var       count   = 0;
			var       source  = parameter.Span;
			for (var i = 0; i < length; i++)
			{
				var element = source[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new T[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = source[indexes[i]];
			}

			return result;
		}
	}

	class SelectSelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>, IActivateMarker<Func<TFrom, TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectSelector(Func<TFrom, TTo> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.Select(_select);
	}

	class SelectManySelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>,
	                                       IActivateMarker<Func<TFrom, IEnumerable<TTo>>>
	{
		readonly Func<TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Func<TFrom, IEnumerable<TTo>> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.SelectMany(_select);
	}
}