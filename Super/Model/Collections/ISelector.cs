using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	public interface ISelector<TFrom, TTo> : ISelect<ReadOnlyMemory<TFrom>, ReadOnlyMemory<TTo>> {}

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

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;

			var list  = new List<TTo>();
			var span  = parameter.Span;
			for (var i = 0; i < length; i++)
			{
				var element = _select(span[i]);
				if (_where(element))
				{
					list.Add(element);
				}
			}

			var result = list.ToArray();
			return result;
		}
	}

	public sealed class SelectWhereDecorator<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly ISelect<ReadOnlyMemory<TFrom>, ReadOnlyMemory<TTo>> _select;
		readonly Func<TTo, bool>  _where;

		public SelectWhereDecorator(ISelect<ReadOnlyMemory<TFrom>, ReadOnlyMemory<TTo>> select, Expression<Func<TTo, bool>> where)
			: this(@select, where.Compile()) {}

		SelectWhereDecorator(ISelect<ReadOnlyMemory<TFrom>, ReadOnlyMemory<TTo>> select, Func<TTo, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var list = new List<TTo>();
			var memory = _select.Get(parameter);
			var span = memory.Span;
			var length = span.Length;
			for (var i = 0; i < length; i++)
			{
				var element = span[i];
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