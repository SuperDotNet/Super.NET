using Super.Model.Sources.Alterations;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Linq.Expressions;

namespace Super.Expressions
{
	sealed class ConvertAlteration : Invocation0<Expression, Type, Expression>, IAlteration<Expression>, IActivateMarker<Type>
	{
		public ConvertAlteration(Type type) : base(Expression.Convert, type) {}
	}
}