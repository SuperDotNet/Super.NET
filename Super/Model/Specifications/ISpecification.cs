namespace Super.Model.Specifications
{
	public interface ISpecification<in T>
	{
		bool IsSatisfiedBy(T parameter);
	}
}