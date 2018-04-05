using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Specifications
{
	sealed class SpecificationAdapter<T> : DelegatedSource<T, bool>, IActivateMarker<ISpecification<T>>
	{
		public SpecificationAdapter(ISpecification<T> specification) : base(specification.IsSatisfiedBy) {}
	}
}