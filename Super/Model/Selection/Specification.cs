using Super.Model.Specifications;
using System;

namespace Super.Model.Selection
{
	public class Specification<TParameter, TResult> : ISpecification<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _source;
		readonly Func<TParameter, bool>   _specification;

		public Specification(ISpecification<TParameter> specification, ISelect<TParameter, TResult> @select)
			: this(specification.IsSatisfiedBy, select.Get) {}

		public Specification(Func<TParameter, bool> specification, Func<TParameter, TResult> source)
		{
			_specification = specification;
			_source        = source;
		}

		public bool IsSatisfiedBy(TParameter parameter) => _specification(parameter);

		public TResult Get(TParameter parameter) => _source(parameter);
	}
}