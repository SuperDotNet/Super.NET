using Super.Model.Specifications;
using System;

namespace Super.Model.Sources
{
	class ValidatedSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _fallback;
		readonly Func<TParameter, TResult> _source;
		readonly Func<TParameter, bool>    _specification;

		public ValidatedSource(ISpecification<TParameter> specification, ISource<TParameter, TResult> source)
			: this(specification, source, Default<TParameter, TResult>.Instance) {}

		public ValidatedSource(ISpecification<TParameter> specification, ISource<TParameter, TResult> source,
		                       ISource<TParameter, TResult> fallback)
			: this(specification.IsSatisfiedBy, source.Get, fallback.Get) {}

		public ValidatedSource(Func<TParameter, bool> specification, Func<TParameter, TResult> source)
			: this(specification, source, Default<TParameter, TResult>.Instance.Get) {}

		public ValidatedSource(Func<TParameter, bool> specification, Func<TParameter, TResult> source,
		                       Func<TParameter, TResult> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TResult Get(TParameter parameter) => _specification(parameter) ? _source(parameter) : _fallback(parameter);
	}
}