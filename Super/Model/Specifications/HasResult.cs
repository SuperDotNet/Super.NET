using Super.Model.Sources;
using System;

namespace Super.Model.Specifications
{
	public class HasResult<TParameter, TResult> : DelegatedSpecification<TParameter>
	{
		readonly static Func<TResult, bool> Assigned = IsAssigned<TResult>.Default.IsSatisfiedBy;

		protected HasResult(Func<TParameter, TResult> source) : this(source, Assigned) {}

		HasResult(Func<TParameter, TResult> source, Func<TResult, bool> result)
			: base(new SelectedParameterSource<TResult, TParameter, bool>(result, source).Get) {}
	}
}