using Super.Model.Selection.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	class OrderByAlteration<T, TMember> : IAlteration<IEnumerable<T>>
	{
		readonly IComparer<TMember> _comparer;
		readonly Func<T, TMember>   _select;

		public OrderByAlteration(Func<T, TMember> select) : this(select, SortComparer<TMember>.Default) {}

		public OrderByAlteration(Func<T, TMember> select, IComparer<TMember> comparer)
		{
			_select   = select;
			_comparer = comparer;
		}

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter is IOrderedEnumerable<T> ordered
			                                                       ? ordered.ThenBy(_select, _comparer)
			                                                       : parameter.OrderBy(_select, _comparer);
	}
}