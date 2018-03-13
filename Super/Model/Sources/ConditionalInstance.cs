using System;
using Super.Model.Specifications;

namespace Super.Model.Sources
{
	public class ConditionalInstance<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly TResult                _false;
		readonly Func<TParameter, bool> _specification;
		readonly TResult                _true;

		public ConditionalInstance(ISpecification<TParameter> specification, TResult @true, TResult @false)
			: this(specification.IsSatisfiedBy, @true, @false) {}

		public ConditionalInstance(Func<TParameter, bool> specification, TResult @true, TResult @false)
		{
			_specification = specification;
			_true          = @true;
			_false         = @false;
		}

		public TResult Get(TParameter parameter) => _specification(parameter) ? _true : _false;
	}
}