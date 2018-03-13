using Super.Model.Specifications;

namespace Super.Model.Sources
{
	public class SpecificationSource<TParameter, TResult> : ISpecification<TParameter, TResult>
	{
		readonly ISource<TParameter, TResult> _source;
		readonly ISpecification<TParameter>   _specification;

		public SpecificationSource(ISpecification<TParameter> specification, ISource<TParameter, TResult> source)
		{
			_specification = specification;
			_source        = source;
		}

		public bool IsSatisfiedBy(TParameter parameter) => _specification.IsSatisfiedBy(parameter);

		public TResult Get(TParameter parameter) => _source.Get(parameter);
	}
}