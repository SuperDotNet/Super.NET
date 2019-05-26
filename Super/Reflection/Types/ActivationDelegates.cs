using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Invocation;
using Super.Runtime.Invocation.Expressions;

namespace Super.Reflection.Types
{
	class ActivationDelegates<T> : ReferenceValueTable<Type, T> where T : Delegate
	{
		public ActivationDelegates(params Type[] parameters) : this(new ActivateExpressions(parameters)) {}

		public ActivationDelegates(IActivateExpressions expressions)
			: this(expressions, expressions.Parameters.Get().Open()) {}

		public ActivationDelegates(ISelect<Type, Expression> select, params ParameterExpression[] expressions)
			: base(new Lambda(expressions).Select(Compiler<T>.Default).To(select.Select).Get) {}

		sealed class Lambda : Invocation0<Expression, IEnumerable<ParameterExpression>, Expression<T>>
		{
			public Lambda(IEnumerable<ParameterExpression> parameter) : base(Expression.Lambda<T>, parameter) {}
		}
	}
}