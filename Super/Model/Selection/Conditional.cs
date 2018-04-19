using Super.Model.Specifications;
using System;

namespace Super.Model.Selection
{
	public class Conditional<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly TResult                _false;
		readonly Func<TParameter, bool> _specification;
		readonly TResult                _true;

		public Conditional(ISpecification<TParameter> specification, TResult @true, TResult @false)
			: this(specification.ToDelegate(), @true, @false) {}

		public Conditional(Func<TParameter, bool> specification, TResult @true, TResult @false)
		{
			_specification = specification;
			_true          = @true;
			_false         = @false;
		}

		public TResult Get(TParameter parameter) => _specification(parameter) ? _true : _false;
	}
}