﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Runtime.Invocation.Expressions
{
	sealed class Instances<T> : Decorated<ConstructorInfo, Expression>
	{
		public static Instances<T> Default { get; } = new Instances<T>();

		Instances() : base(new Instances(Parameters<T>.Default)) {}
	}

	sealed class Instances : ISelect<ConstructorInfo, Expression>
	{
		public static Instances Default { get; } = new Instances();

		Instances() :
			this(new Delegated<ConstructorInfo, IEnumerable<Expression>>(Empty<Expression>.Enumerable.Accept)) {}

		readonly ISelect<ConstructorInfo, IEnumerable<Expression>> _parameters;

		public Instances(ISelect<ConstructorInfo, IEnumerable<Expression>> parameters) => _parameters = parameters;

		public Expression Get(ConstructorInfo parameter) => new Activation(_parameters.Get(parameter)).Get(parameter);
	}
}