namespace Super.Model.Specifications
{
	public class InverseSpecification<T> : ISpecification<T>
	{
		readonly ISpecification<T> _specification;

		public InverseSpecification(ISpecification<T> inner) => _specification = inner;

		public bool IsSatisfiedBy(T parameter) => !_specification.IsSatisfiedBy(parameter);
	}
}