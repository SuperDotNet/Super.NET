using Super.Model.Selection.Stores;
using Super.Runtime.Invocation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Reflection.Types
{
	class ActivationDelegates<T> : ReferenceValueTable<Type, T> where T : Delegate
	{
		public ActivationDelegates(params Type[] parameters) : this(new ActivateExpressions(parameters)) {}

		public ActivationDelegates(IActivateExpressions expressions)
			: base(new Lambda(expressions.Parameters.Get().Open()).Select(x => x.Compile())
			                                                .To(expressions.Select)
			                                                .Get) {}

		sealed class Lambda : Invocation0<Expression, IEnumerable<ParameterExpression>, Expression<T>>
		{
			public Lambda(IEnumerable<ParameterExpression> parameter) : base(Expression.Lambda<T>, parameter) {}
		}
	}
}