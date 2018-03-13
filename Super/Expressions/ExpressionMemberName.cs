using System.Linq.Expressions;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Expressions
{
	public sealed class ExpressionMemberName : ISource<LambdaExpression, MemberInfo>
	{
		public static ExpressionMemberName Default { get; } = new ExpressionMemberName();

		ExpressionMemberName() {}

		public MemberInfo Get(LambdaExpression parameter)
			=> (parameter.Body.AsTo<UnaryExpression, Expression>(unaryExpression => unaryExpression.Operand) ??
			    parameter.Body)
			   .To<MemberExpression>()
			   .Member;
	}
}