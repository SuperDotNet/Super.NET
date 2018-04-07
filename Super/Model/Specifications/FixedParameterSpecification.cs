namespace Super.Model.Specifications
{
	sealed class FixedParameterSpecification<T> : ISpecification<ISpecification<T>>
	{
		readonly T _parameter;

		public FixedParameterSpecification(T parameter) => _parameter = parameter;

		public bool IsSatisfiedBy(ISpecification<T> parameter) => parameter.IsSatisfiedBy(_parameter);
	}
}
