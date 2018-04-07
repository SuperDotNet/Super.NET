using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class Activation : Invocation0<ConstructorInfo, IEnumerable<Expression>, Expression>
	{
		public Activation(IEnumerable<Expression> parameter) : base(Expression.New, parameter) {}
	}
}