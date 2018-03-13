using System.Linq.Expressions;
using Super.Runtime;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class Lambda<T> : Invocation0<Expression, ParameterExpression[], Expression<T>>
	{
		public static Lambda<T> Default { get; } = new Lambda<T>();

		Lambda() : this(Empty<ParameterExpression>.Array) {}

		public Lambda(params ParameterExpression[] parameters) : base(Expression.Lambda<T>, parameters) {}
	}
}