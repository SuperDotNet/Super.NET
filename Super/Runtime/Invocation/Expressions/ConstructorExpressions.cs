using Super.Model.Selection;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class ConstructorExpressions<T> : DecoratedSelect<ConstructorInfo, Expression>
	{
		public static ConstructorExpressions<T> Default { get; } = new ConstructorExpressions<T>();

		ConstructorExpressions() : base(new ConstructorExpressions(ConstructorParameters<T>.Default)) {}
	}

	sealed class ConstructorExpressions : ISelect<ConstructorInfo, Expression>
	{
		public static ConstructorExpressions Default { get; } = new ConstructorExpressions();

		ConstructorExpressions() :
			this(new Select<ConstructorInfo, IEnumerable<Expression>>(Empty<Expression>.Enumerable.Accept)) {}

		readonly ISelect<ConstructorInfo, IEnumerable<Expression>> _parameters;

		public ConstructorExpressions(ISelect<ConstructorInfo, IEnumerable<Expression>> parameters) => _parameters = parameters;

		public Expression Get(ConstructorInfo parameter) => new New(_parameters.Get(parameter)).Get(parameter);
	}
}