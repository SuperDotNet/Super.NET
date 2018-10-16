using Super.Model.Selection;
using System.Linq.Expressions;

namespace Super.Runtime.Invocation.Expressions
{
	class Delegates<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
	{
		public Delegates(ISelect<TParameter, Expression> @select) : this(@select, Empty<ParameterExpression>.Array) {}

		protected Delegates(ISelect<TParameter, Expression> @select, params ParameterExpression[] parameters)
			: base(@select /*.Select(alteration)
			              */
			       .Select(new Lambda<TResult>(parameters))
			       .Compile()) {}
	}
}