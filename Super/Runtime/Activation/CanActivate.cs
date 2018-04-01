using Super.Model.Specifications;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class CanActivate : ISpecification<TypeInfo>
	{
		readonly static TypeInfo GeneralObject = typeof(object).GetTypeInfo();

		public static CanActivate Default { get; } = new CanActivate();

		CanActivate() {}

		public bool IsSatisfiedBy(TypeInfo parameter)
			=> !parameter.IsAbstract && parameter.IsClass && !parameter.Equals(GeneralObject);
	}
}