using System;
using System.Linq.Expressions;
using Super.Model.Selection.Alterations;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class ConvertAlteration : Invocation0<Expression, Type, Expression>, IAlteration<Expression>, IActivateMarker<Type>
	{
		public ConvertAlteration(Type type) : base(Expression.Convert, type) {}
	}
}