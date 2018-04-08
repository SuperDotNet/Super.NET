using System;
using System.Linq.Expressions;
using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	class Delegates<TParameter, TResult> : Decorated<TParameter, TResult>
	{
		public Delegates(ISelect<TParameter, Expression> @select) : this(@select, Empty<ParameterExpression>.Array) {}

		public Delegates(ISelect<TParameter, Expression> @select, params ParameterExpression[] expressions)
			: this(@select, ReturnType<TResult>.Default.Get(), expressions) {}

		public Delegates(ISelect<TParameter, Expression> @select, Type resultType, params ParameterExpression[] parameters)
			: base(ConvertExpressions.Default
			                        .Get(resultType)
			                        .Out(new Lambda<TResult>(parameters))
			                        .Out(Compiler<TResult>.Default)
			                        .In(@select)) {}
	}
}