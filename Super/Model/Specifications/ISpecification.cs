using Super.Runtime.Activation;
using System.Reactive;

namespace Super.Model.Specifications
{
	public interface ISpecification : ISpecification<Unit> {}

	public interface ISpecification<in T>
	{
		bool IsSatisfiedBy(T parameter);
	}

	public interface IAny : ISpecification, ISpecification<object> {}

	sealed class Any : IAny, IActivateMarker<ISpecification>
	{
		readonly ISpecification _specification;

		public Any(ISpecification specification) => _specification = specification;

		public bool IsSatisfiedBy(object _) => _specification.IsSatisfiedBy();

		public bool IsSatisfiedBy(Unit _) => _specification.IsSatisfiedBy();
	}
}