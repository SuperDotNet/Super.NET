using System;
using System.Linq.Expressions;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Parameter<T> : Source<ParameterExpression>
	{
		readonly static ISelect<string, ParameterExpression> Select = new Parameter(Types<T>.Identity).ToReferenceStore();

		public static Parameter<T> Default { get; } = new Parameter<T>();

		public Parameter(string name = "parameter") : base(Select.Get(name)) {}
	}

	sealed class Parameter : Invocation1<Type, string, ParameterExpression>
	{
		public static Parameter Default { get; } = new Parameter();

		Parameter() : this(Types<object[]>.Identity) {}

		public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
	}
}