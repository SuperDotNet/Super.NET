namespace Super.Model.Specifications
{
	public sealed class Never : DelegatedSpecification<object>
	{
		public static Never Default { get; } = new Never();

		Never() : base(Never<object>.Default.IsSatisfiedBy) {}
	}

	public sealed class Never<T> : FixedResultSpecification<T>
	{
		public static ISpecification<T> Default { get; } = new Never<T>();

		Never() : base(false) {}
	}
}