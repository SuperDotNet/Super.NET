using System;
using Super.Model.Selection;
using Super.Model.Specifications;

namespace Super.Model.Collections
{
	class ArrayWhere<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;

		public ArrayWhere(ISelect<TFrom, TTo> select, ISpecification<TTo> where) : this(select.Get, where.IsSatisfiedBy) {}

		public ArrayWhere(Func<TFrom, TTo> select, Func<TTo, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var store = new TTo[parameter.Length];
			//var valid = new Array<TTo>(store);
			var count = 0;
			for (var i = 0; i < store.Length; i++)
			{
				var element = _select(parameter[i]);
				if (_where(element))
				{
					store[count++] = element;
				}
			}

			var to     = new TTo[count];
			var result = new Array<TTo>(to);
			Array.Copy(store, to, count);
			return result;
		}
	}
}