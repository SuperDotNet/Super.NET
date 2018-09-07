using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public class Selection<TFrom, TTo> : IShape<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public Selection(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public Selection(Func<TFrom, TTo> select) => _select = select;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];
			var count  = 0;
			var span   = parameter.Span;

			for (var i = 0; i < length; i++)
			{
				result[count++] = _select(span[i]);
			}

			return result;
		}
	}

	public class Where<T> : IShape<T, T>
	{
		readonly Func<T, bool> _where;

		public Where(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public Where(Func<T, bool> where) => _where = where;

		public ReadOnlyMemory<T> Get(ReadOnlyMemory<T> parameter)
		{
			Span<T> store = new T[parameter.Length];
			var     count = 0;
			var     span  = parameter.Span;

			for (var i = 0; i < store.Length; i++)
			{
				var element = span[i];
				if (_where(element))
				{
					store[count++] = element;
				}
			}

			var result = store.Slice(0, count).ToArray();
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