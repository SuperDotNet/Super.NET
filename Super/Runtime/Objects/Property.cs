using System;
using System.Linq.Expressions;
using Super.Model.Selection;
using Super.Runtime.Invocation.Expressions;

namespace Super.Runtime.Objects
{
	sealed class Property<T> : Select<T, Pair<string, object>>, IProperty<T>
	{
		public Property(Expression<Func<T, object>> expression)
			: base(expression.Compile().ToSelect().Select(new Pairing(expression.GetMemberInfo().Name))) {}
	}
}