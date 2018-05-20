using System;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	sealed class SelectRaw<TFrom, TTo> : ISelect<TFrom[], TTo[]>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectRaw(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public SelectRaw(Func<TFrom, TTo> select) => _select = select;

		public TTo[] Get(TFrom[] parameter)
		{
			var source = parameter;
			var length = source.Length;
			var result = new TTo[length];
			for (var i = 0; i < length; ++i)
			{
				result[i] = _select(source[i]);
			}

			return result;
		}
	}
}