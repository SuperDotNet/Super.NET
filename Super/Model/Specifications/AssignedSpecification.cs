namespace Super.Model.Specifications
{
	sealed class AssignedSpecification<T> : ISpecification<T>
	{
		public static AssignedSpecification<T> Default { get; } = new AssignedSpecification<T>();

		AssignedSpecification() {}

		public bool IsSatisfiedBy(T parameter) => parameter != null;
	}
}