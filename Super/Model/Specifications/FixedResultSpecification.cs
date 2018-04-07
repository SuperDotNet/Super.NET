using Super.Runtime.Activation;

namespace Super.Model.Specifications
{
	sealed class FixedResultSpecification : ISpecification<object>, IActivateMarker<bool>
	{
		readonly bool _result;

		public FixedResultSpecification(bool result) => _result = result;

		public bool IsSatisfiedBy(object _) => _result;
	}
}