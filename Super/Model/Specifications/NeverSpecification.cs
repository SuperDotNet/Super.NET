namespace Super.Model.Specifications
{
	public sealed class NeverSpecification : DelegatedSpecification<object>
	{
		public static NeverSpecification Default { get; } = new NeverSpecification();

		NeverSpecification() : base(NeverSpecification<object>.Default.IsSatisfiedBy) {}
	}

	public sealed class NeverSpecification<T> : ISpecification<T>
	{
		public static NeverSpecification<T> Default { get; } = new NeverSpecification<T>();

		NeverSpecification() {}

		public bool IsSatisfiedBy(T parameter) => false;
	}
}