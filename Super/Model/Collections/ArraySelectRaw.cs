using System;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	sealed class ArraySelectRaw<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelectRaw(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelectRaw(Func<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var source = parameter.Get();
			var length = source.Length;
			var store  = new TTo[length];
			for (var i = 0; i < length; ++i)
			{
				store[i] = _select(source[i]);
			}

			var result = new Array<TTo>(store);
			return result;
		}
	}
}