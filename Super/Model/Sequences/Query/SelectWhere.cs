using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Super.Model.Selection;
using Super.Model.Specifications;

namespace Super.Model.Sequences.Query {
	public sealed class SelectWhere<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;

		public SelectWhere(ISelect<TFrom, TTo> select) : this(@select, Always<TTo>.Default) {}

		public SelectWhere(Func<TFrom, TTo> select, Expression<Func<TTo, bool>> specification)
			: this(@select, specification.Compile()) {}

		public SelectWhere(ISelect<TFrom, TTo> select, ISpecification<TTo> where) :
			this(select.Get, where.IsSatisfiedBy) {}

		SelectWhere(Func<TFrom, TTo> select, Func<TTo, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;

			var list = new List<TTo>();
			for (var i = 0; i < length; i++)
			{
				var element = _select(parameter[i]);
				if (_where(element))
				{
					list.Add(element);
				}
			}

			var result = list.ToArray();
			return result;
		}
	}
}