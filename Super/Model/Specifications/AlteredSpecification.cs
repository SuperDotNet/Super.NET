using Super.Model.Sources.Alterations;

namespace Super.Model.Specifications
{
	public sealed class AlteredSpecification<T> : ISpecification<T>
	{
		readonly IAlteration<T>    _alteration;
		readonly ISpecification<T> _specification;

		public AlteredSpecification(ISpecification<T> specification, IAlteration<T> alteration)
		{
			_specification = specification;
			_alteration    = alteration;
		}

		public bool IsSatisfiedBy(T parameter) => _specification.IsSatisfiedBy(_alteration.Get(parameter));
	}
}