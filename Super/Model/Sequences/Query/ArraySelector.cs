using Super.Model.Selection;
using System;

namespace Super.Model.Sequences.Query
{
	public sealed class ArraySelector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelector(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelector(Func<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];

			for (var i = 0; i < length; i++)
			{
				result[i] = _select(parameter[i]);
			}

			return result;
		}
	}
}