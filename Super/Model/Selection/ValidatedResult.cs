using Super.Model.Specifications;
using System;

namespace Super.Model.Selection
{
	class ValidatedResult<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _fallback;
		readonly Func<TParameter, TResult> _source;
		readonly Func<TResult, bool>       _specification;

		public ValidatedResult(ISpecification<TResult> specification, ISelect<TParameter, TResult> @select,
		                       ISelect<TParameter, TResult> fallback)
			: this(specification.IsSatisfiedBy, @select.Get, fallback.Get) {}

		public ValidatedResult(Func<TResult, bool> specification, Func<TParameter, TResult> source,
		                       Func<TParameter, TResult> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TResult Get(TParameter parameter)
		{
			var candidate = _source(parameter);
			var result    = _specification(candidate) ? candidate : _fallback(parameter);
			return result;
		}
	}
}