using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Testing.Framework
{
	public sealed class SingletonQuery : ISource<Type, IEnumerable<IMethod>>, IMethodQuery, ISpecification<Type>
	{
		public static SingletonQuery Default { get; } = new SingletonQuery();
		SingletonQuery() {}

		public IEnumerable<IMethod> Get(Type parameter)
		{
			if (HasSingletonProperty.Default.IsSatisfiedBy(parameter))
			{
				yield return new SingletonMethod(parameter);
			}
		}

		IEnumerable<IMethod> IMethodQuery.SelectMethods(Type type) => Get(type);

		bool ISpecification<Type>.IsSatisfiedBy(Type parameter) => false;
	}
}