using Super.ExtensionMethods;
using Super.Model.Specifications;
using System;

namespace Super.Model.Sources
{
	public class SpecificationSource<TParameter, TResult> : ISpecification<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _source;
		readonly Func<TParameter, bool>   _specification;

		public SpecificationSource(ISpecification<TParameter> specification, ISource<TParameter, TResult> source)
			: this(specification.ToDelegate(), source.ToDelegate()) {}

		public SpecificationSource(Func<TParameter, bool> specification, Func<TParameter, TResult> source)
		{
			_specification = specification;
			_source        = source;
		}

		public bool IsSatisfiedBy(TParameter parameter) => _specification(parameter);

		public TResult Get(TParameter parameter) => _source(parameter);
	}
}