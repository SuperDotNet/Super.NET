using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Runtime.Activation;

namespace Super.Application.Hosting.xUnit
{
	public sealed class SingletonQuery : ISelect<Type, IEnumerable<IMethod>>, IMethodQuery
	{
		public static SingletonQuery Default { get; } = new SingletonQuery();

		SingletonQuery() : this(HasSingletonProperty.Default) {}

		readonly ICondition<Type> _condition;

		public SingletonQuery(ICondition<Type> condition) => _condition = condition;

		IEnumerable<IMethod> IMethodQuery.SelectMethods(Type type) => Get(type);

		public IEnumerable<IMethod> Get(Type parameter)
		{
			if (_condition.Get(parameter))
			{
				yield return new SingletonMethod(parameter);
			}
		}
	}
}