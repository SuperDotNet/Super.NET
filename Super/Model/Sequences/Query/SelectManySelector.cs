using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Sequences.Query
{
	class SelectManySelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>
	{
		readonly Func<TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Func<TFrom, IEnumerable<TTo>> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.SelectMany(_select);
	}
}