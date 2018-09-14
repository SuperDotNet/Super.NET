using Super.Model.Selection;
using System;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	public sealed class ExpressionSelector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly static ISelect<Expression<Func<TFrom, TTo>>, Action<TFrom[], TTo[], int, int>> Select
			= InlineSelections<TFrom, TTo>.Default.Compile();

		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ExpressionSelector(Expression<Func<TFrom, TTo>> select) : this(Select.Get(select)) {}

		public ExpressionSelector(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var result  = new TTo[length];

			_iterate(parameter.Segment()?.Array, result, 0, length);

			return result;
		}
	}
}