using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Collections.Commands;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using Super.Model.Selection.Conditions;

namespace Super.Application.Hosting.xUnit
{
	public sealed class SingletonQuery : ISelect<Type, IEnumerable<IMethod>>, IMethodQuery
	{
		public static SingletonQuery Default { get; } = new SingletonQuery();
		SingletonQuery() : this(HasSingletonProperty.Default) {}

		readonly ICondition<Type> _condition;

		public SingletonQuery(ICondition<Type> condition) => _condition = condition;

		public IEnumerable<IMethod> Get(Type parameter)
		{
			if (_condition.Get(parameter))
			{
				yield return new SingletonMethod(parameter);
			}
		}

		IEnumerable<IMethod> IMethodQuery.SelectMethods(Type type) => Get(type);
	}

	sealed class SelectCustomizations : Select<IFixture, IList<ISpecimenBuilder>>
	{
		public static SelectCustomizations Default { get; } = new SelectCustomizations();

		SelectCustomizations() : base(x => x.Customizations) {}
	}

	sealed class SingletonCustomization : InsertCustomization
	{
		public static SingletonCustomization Default { get; } = new SingletonCustomization();

		SingletonCustomization() : base(new MethodInvoker(SingletonQuery.Default)) {}
	}

	public class InsertCustomization : DecoratedCommand<IFixture>, ICustomization
	{
		public InsertCustomization(ISpecimenBuilder specimen) : this(specimen, x => 0) {}

		public InsertCustomization(ISpecimenBuilder specimen, Func<IList<ISpecimenBuilder>, int> index)
			: base(SelectCustomizations.Default.Terminate(new InsertIntoList<ISpecimenBuilder>(specimen, index))) {}

		public void Customize(IFixture fixture)
		{
			Execute(fixture);
		}
	}
}