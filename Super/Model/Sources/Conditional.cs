using System;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Runtime;

namespace Super.Model.Sources
{
	public class Conditional<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TParameter, TResult> _source, _fallback;
		readonly Func<TParameter, bool>    _specification;

		public Conditional(ISpecification<TParameter> specification, ISource<TParameter, TResult> source)
			: this(specification, source, Default<TParameter, TResult>.Instance) {}

		public Conditional(ISpecification<TParameter> specification, ISource<TParameter, TResult> source,
		                   ISource<TParameter, TResult> fallback)
			: this(specification.Adapt().ToDelegate(), source.ToDelegate(), fallback.ToDelegate()) {}

		public Conditional(Func<TParameter, bool> specification, Func<TParameter, TResult> source,
		                   Func<TParameter, TResult> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TResult Get(TParameter parameter) => _specification(parameter) ? _source(parameter) : _fallback(parameter);
	}
}