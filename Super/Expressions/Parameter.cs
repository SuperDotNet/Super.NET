﻿using System;
using System.Linq.Expressions;
using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Invocation;

namespace Super.Expressions
{
	sealed class Parameter<T> : Instance<ParameterExpression>
	{
		readonly static ISource<string, ParameterExpression> Source = new Parameter(Types<T>.Identity).ToReferenceStore();

		public static Parameter<T> Default { get; } = new Parameter<T>();

		public Parameter(string name = "parameter") : base(Source.Get(name)) {}
	}

	sealed class Parameter : Invocation1<Type, string, ParameterExpression>
	{
		public static Parameter Default { get; } = new Parameter();

		Parameter() : this(Types<object[]>.Identity) {}

		public Parameter(Type parameter) : base(Expression.Parameter, parameter) {}
	}
}