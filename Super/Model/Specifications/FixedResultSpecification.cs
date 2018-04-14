using Super.Runtime.Activation;

namespace Super.Model.Specifications
{
	public class FixedResultSpecification<T> : ISpecification<T>, IActivateMarker<bool>
	{
		readonly bool _result;

		public FixedResultSpecification(bool result) => _result = result;

		public bool IsSatisfiedBy(T _) => _result;
	}
}