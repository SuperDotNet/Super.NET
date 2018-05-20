using System;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	sealed class ArraySelectInline<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ArraySelectInline(Expression<Func<TFrom, TTo>> select)
			: this(InlineSelections<TFrom, TTo>.Default.Compile().Get(select)) {}

		public ArraySelectInline(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;
			var store  = new TTo[length];
			_iterate(parameter._source, store, 0, length);

			var result = new Array<TTo>(store);
			return result;
		}
	}
}