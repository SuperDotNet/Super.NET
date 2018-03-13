using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime;

namespace Super.Expressions
{
	sealed class Instances<T> : DecoratedSource<ConstructorInfo, Expression>
	{
		public static Instances<T> Default { get; } = new Instances<T>();

		Instances() : base(new Instances(Parameters<T>.Default)) {}
	}

	sealed class Instances : ISource<ConstructorInfo, Expression>
	{
		public static Instances Default { get; } = new Instances();

		Instances() :
			this(new DelegatedSource<ConstructorInfo, IEnumerable<Expression>>(Empty<Expression>.Enumerable.Accept)) {}

		readonly ISource<ConstructorInfo, IEnumerable<Expression>> _parameters;

		public Instances(ISource<ConstructorInfo, IEnumerable<Expression>> parameters) => _parameters = parameters;

		public Expression Get(ConstructorInfo parameter) => new New(_parameters.Get(parameter)).Get(parameter);
	}
}