using Super.Model.Selection;
using System;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	sealed class ArraySelectInline<TFrom, TTo> : IShape<TFrom, TTo>
	{
		readonly static ISelect<Expression<Func<TFrom, TTo>>, Action<TFrom[], TTo[], int, int>> Select
			= InlineSelections<TFrom, TTo>.Default.Compile();

		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ArraySelectInline(Expression<Func<TFrom, TTo>> select) : this(Select.Get(select)) {}

		public ArraySelectInline(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result  = new TTo[length];
			_iterate(parameter.ToArray(), result, 0, length);

			return result;
		}
	}
}