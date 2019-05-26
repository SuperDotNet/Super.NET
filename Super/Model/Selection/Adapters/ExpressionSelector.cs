using System;
using System.Linq.Expressions;
using Super.Runtime.Invocation.Expressions;

namespace Super.Model.Selection.Adapters
{
	public class ExpressionSelector<_, T> : Selector<_, Expression<T>>
	{
		public ExpressionSelector(ISelect<_, Expression<T>> subject) : base(subject) {}

		public Selector<_, T> Compile() => Select(Compiler<T>.Default);
	}

	public class ExpressionSelector<_> : Selector<_, Expression>
	{
		public ExpressionSelector(ISelect<_, Expression> subject) : base(subject) {}

		public ExpressionSelector<_, TTo> Select<TTo>(ISelect<Expression, Expression<TTo>> select)
			=> Select(select.Get);

		public ExpressionSelector<_, TTo> Select<TTo>(Func<Expression, Expression<TTo>> select)
			=> new ExpressionSelector<_, TTo>(Get().Select(select));
	}
}