using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Specifications
{
	sealed class SpecificationSourceAdapter<T> : DelegatedSource<T, bool>, IActivateMarker<ISpecification<T>>
	{
		public SpecificationSourceAdapter(ISpecification<T> specification) : base(specification.IsSatisfiedBy) {}
	}
}