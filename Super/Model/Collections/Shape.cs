using Super.Model.Selection;
using Super.Model.Specifications;
using System;

namespace Super.Model.Collections
{
	class Shape<TFrom, TTo> : IShape<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;

		public Shape(ISelect<TFrom, TTo> select, ISpecification<TTo> where) : this(select.Get, where.IsSatisfiedBy) {}

		public Shape(Func<TFrom, TTo> select, Func<TTo, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var store = new TTo[parameter.Length];
			var count = 0;
			var span = parameter.Span;
			for (var i = 0; i < store.Length; i++)
			{
				var element = _select(span[i]);
				if (_where(element))
				{
					store[count++] = element;
				}
			}

			var result = new TTo[count];
			Array.Copy(store, result, count);
			return result;

		}
	}
}