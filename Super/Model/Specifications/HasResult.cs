using System;
using Super.Model.Selection;

namespace Super.Model.Specifications
{
	public class HasResult<TParameter, TResult> : DelegatedSpecification<TParameter>
	{
		readonly static Func<TResult, bool> Assigned = IsAssigned<TResult>.Default.IsSatisfiedBy;

		protected HasResult(Func<TParameter, TResult> source) : this(source, Assigned) {}

		HasResult(Func<TParameter, TResult> source, Func<TResult, bool> result)
			: base(new Parameter<TResult, TParameter, bool>(result, source).Get) {}
	}
}