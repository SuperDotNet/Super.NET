namespace Super.Model.Specifications
{
	sealed class IsAssigned<T> : ISpecification<T>
	{
		public static IsAssigned<T> Default { get; } = new IsAssigned<T>();

		IsAssigned() {}

		public bool IsSatisfiedBy(T parameter) => parameter != null;
	}
}