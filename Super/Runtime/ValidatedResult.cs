using System;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Runtime
{
	class ValidatedResult<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _fallback;
		readonly Func<TParameter, TResult> _source;
		readonly Func<TResult, bool>       _specification;

		public ValidatedResult(ISpecification<TResult> specification, ISource<TParameter, TResult> source,
		                       ISource<TParameter, TResult> fallback)
			: this(specification.IsSatisfiedBy, source.Get, fallback.Get) {}

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