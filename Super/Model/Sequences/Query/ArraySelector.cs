using System;

namespace Super.Model.Sequences.Query
{
	public sealed class ArraySelector<TFrom, TTo> : ISelectView<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelector(Func<TFrom, TTo> select) => _select = select;

		public ArrayView<TTo> Get(ArrayView<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];

			for (var i = parameter.Start; i < length; i++)
			{
				result[i] = _select(parameter.Array[i]);
			}

			return new ArrayView<TTo>(result, 0, length);
		}
	}
}