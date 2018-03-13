using System;
using System.Linq.Expressions;
using Super.Model.Sources;
using Super.Model.Sources.Tables;
using Super.Reflection;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class Parameter<T> : FixedParameterSource<string, ParameterExpression>
	{
		public static Parameter<T> Default { get; } = new Parameter<T>();

		public Parameter(string name = "parameter") :
			base(ReferenceTables<string, ParameterExpression>.Default.Get(new Parameter(Types<T>.Identity).Get), name) {}
	}

	sealed class Parameter : Invocation1<Type, string, ParameterExpression>
	{
		public static Parameter Default { get; } = new Parameter();

		Parameter() : this(Types<object[]>.Identity) {}

		public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
	}
}