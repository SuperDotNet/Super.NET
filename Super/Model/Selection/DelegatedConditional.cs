using Super.Model.Specifications;
using System;

namespace Super.Model.Selection
{
	public class DelegatedConditional<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _source, _fallback;
		readonly Func<TParameter, bool>    _specification;

		public DelegatedConditional(ISpecification<TParameter> specification, ISelect<TParameter, TResult> @select)
			: this(specification, @select, Default<TParameter, TResult>.Instance) {}

		public DelegatedConditional(ISpecification<TParameter> specification, ISelect<TParameter, TResult> @select,
		                   ISelect<TParameter, TResult> fallback)
			: this(specification.ToDelegate(), @select.ToDelegate(), fallback.ToDelegate()) {}

		public DelegatedConditional(Func<TParameter, bool> specification, Func<TParameter, TResult> source,
		                   Func<TParameter, TResult> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TResult Get(TParameter parameter) => _specification(parameter) ? _source(parameter) : _fallback(parameter);
	}
}