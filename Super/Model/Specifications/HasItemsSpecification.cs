using System.Collections;

namespace Super.Model.Specifications
{
	public sealed class HasItemsSpecification : ISpecification<ICollection>
	{
		public static HasItemsSpecification Default { get; } = new HasItemsSpecification();

		HasItemsSpecification() {}

		public bool IsSatisfiedBy(ICollection parameter) => parameter.Count > 0;
	}
}