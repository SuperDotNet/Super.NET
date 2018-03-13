using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class New : Invocation0<ConstructorInfo, IEnumerable<Expression>, Expression>
	{
		public New(IEnumerable<Expression> parameter) : base(Expression.New, parameter) {}
	}
}