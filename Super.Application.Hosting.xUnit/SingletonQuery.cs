using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Application.Hosting.xUnit
{
	public sealed class SingletonQuery : ISelect<Type, IEnumerable<IMethod>>, IMethodQuery, ISpecification<Type>
	{
		public static SingletonQuery Default { get; } = new SingletonQuery();
		SingletonQuery() : this(HasSingletonProperty.Default) {}

		readonly ISpecification<Type> _specification;

		public SingletonQuery(ISpecification<Type> specification) => _specification = specification;

		public IEnumerable<IMethod> Get(Type parameter)
		{
			if (_specification.IsSatisfiedBy(parameter))
			{
				yield return new SingletonMethod(parameter);
			}
		}

		IEnumerable<IMethod> IMethodQuery.SelectMethods(Type type) => Get(type);

		bool ISpecification<Type>.IsSatisfiedBy(Type parameter) => false;
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
			: base(new InsertIntoList<ISpecimenBuilder>(specimen, index).Select(SelectCustomizations.Default)) {}

		public void Customize(IFixture fixture)
		{
			Execute(fixture);
		}
	}
}