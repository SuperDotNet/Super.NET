using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;
using Super.Model.Sources;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Application.Hosting.xUnit
{
	sealed class SingletonMethod : FixedSelection<Type, object>, IMethod
	{
		public SingletonMethod(Type parameter) : base(Singletons.Default, parameter) { }

		public IEnumerable<ParameterInfo> Parameters { get; } = Empty<ParameterInfo>.Enumerable;

		object IMethod.Invoke(IEnumerable<object> parameters) => Get();
	}
}