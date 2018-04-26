using System.Reactive;
using Super.Runtime.Activation;

namespace Super.Model.Specifications {
	sealed class Any : IAny, IActivateMarker<ISpecification<Unit>>
	{
		readonly ISpecification<Unit> _specification;

		public Any(ISpecification<Unit> specification) => _specification = specification;

		public bool IsSatisfiedBy(object _) => _specification.IsSatisfiedBy();

		public bool IsSatisfiedBy(Unit _) => _specification.IsSatisfiedBy();
	}
}