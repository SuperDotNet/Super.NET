using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Instances<T> : DecoratedSelect<ConstructorInfo, Expression>
	{
		public static Instances<T> Default { get; } = new Instances<T>();

		Instances() : base(new Instances(Parameters<T>.Default)) {}
	}

	sealed class Instances : ISelect<ConstructorInfo, Expression>
	{
		public static Instances Default { get; } = new Instances();

		Instances() :
			this(new Select<ConstructorInfo, IEnumerable<Expression>>(Empty<Expression>.Enumerable.Accept)) {}

		readonly ISelect<ConstructorInfo, IEnumerable<Expression>> _parameters;

		public Instances(ISelect<ConstructorInfo, IEnumerable<Expression>> parameters) => _parameters = parameters;

		public Expression Get(ConstructorInfo parameter) => new New(_parameters.Get(parameter)).Get(parameter);
	}
}