using System;
using System.Linq.Expressions;
using Super.Model.Sources.Alterations;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class ConvertAlteration : Invocation0<Expression, Type, Expression>, IAlteration<Expression>
	{
		public ConvertAlteration(Type type) : base(Expression.Convert, type) {}
	}
}