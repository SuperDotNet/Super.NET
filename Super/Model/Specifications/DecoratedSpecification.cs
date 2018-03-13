namespace Super.Model.Specifications
{
	public class DecoratedSpecification<T> : DelegatedSpecification<T>
	{
		public DecoratedSpecification(ISpecification<T> specification) : base(specification.IsSatisfiedBy) {}
	}
}