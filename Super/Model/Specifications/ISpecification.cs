using System.Reactive;

namespace Super.Model.Specifications
{
	public interface ISpecification : ISpecification<Unit> {}

	public interface ISpecification<in T>
	{
		bool IsSatisfiedBy(T parameter);
	}
}