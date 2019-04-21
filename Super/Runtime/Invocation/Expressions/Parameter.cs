using Super.Model.Results;
using Super.Model.Selection.Stores;
using Super.Reflection.Types;
using System;
using System.Linq.Expressions;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Parameters<T> : ReferenceValueStore<string, ParameterExpression>
	{
		public static Parameters<T> Default { get; } = new Parameters<T>();

		Parameters() : base(new Parameter(Type<T>.Instance).Get) {}
	}

	sealed class Parameter<T> : FixedSelectedSingleton<string, ParameterExpression>
	{
		public static Parameter<T> Default { get; } = new Parameter<T>();

		public Parameter(string name = "parameter") : base(Parameters<T>.Default, name) {}
	}

	sealed class Parameter : Invocation1<Type, string, ParameterExpression>
	{
		public static Parameter Default { get; } = new Parameter();

		Parameter() : this(Type<object[]>.Instance) {}

		public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
	}
}