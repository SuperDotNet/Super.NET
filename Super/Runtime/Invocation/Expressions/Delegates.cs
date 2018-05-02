using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Linq.Expressions;

namespace Super.Runtime.Invocation.Expressions
{
	class Delegates<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
	{
		public Delegates(ISelect<TParameter, Expression> @select) : this(@select, Empty<ParameterExpression>.Array) {}

		public Delegates(ISelect<TParameter, Expression> @select, params ParameterExpression[] expressions)
			: this(@select, ReturnType<TResult>.Default.Get(), expressions) {}

		public Delegates(ISelect<TParameter, Expression> @select, Type resultType, params ParameterExpression[] parameters)
			: this(select, ConvertExpressions.Default.Get(resultType), parameters) {}

		public Delegates(ISelect<TParameter, Expression> @select, IAlteration<Expression> alteration, params ParameterExpression[] parameters)
			: base(@select.Select(alteration)
			              .Select(new Lambda<TResult>(parameters))
			              .Compile()) {}
	}
}