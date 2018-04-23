using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using System;
using System.Linq.Expressions;

namespace Super.Runtime.Invocation.Expressions
{
	static class Implementations<T>
	{
		public static ISelect<string, ParameterExpression> Select { get; } = new Parameter(Type<T>.Instance).ToReferenceStore();
	}

	sealed class Parameter<T> : Source<ParameterExpression>
	{
		public static Parameter<T> Default { get; } = new Parameter<T>();

		public Parameter(string name = "parameter") : base(Implementations<T>.Select.Get(name)) {}
	}

	sealed class Parameter : Invocation1<Type, string, ParameterExpression>
	{
		public static Parameter Default { get; } = new Parameter();

		Parameter() : this(Type<object[]>.Instance) {}

		public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
	}
}