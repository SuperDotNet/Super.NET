namespace Super.Model.Specifications
{
	public sealed class Always<T> : FixedResultSpecification<T>
	{
		public static ISpecification<T> Default { get; } = new Always<T>();

		Always() : base(true) {}
	}
}