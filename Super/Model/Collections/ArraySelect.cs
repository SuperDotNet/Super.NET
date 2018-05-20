using System;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	sealed class ArraySelect<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelect(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelect(Func<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;
			var store  = new TTo[length];
			for (var i = 0; i < length; ++i)
			{
				store[i] = _select(parameter[i]);
			}

			var result = new Array<TTo>(store);
			return result;
		}
	}
}