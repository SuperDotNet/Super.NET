using System;
using Super.ExtensionMethods;

namespace Super.Model.Specifications
{
	public class HasResult<TParameter, TResult> : DelegatedSpecification<TParameter>
	{
		readonly static Func<TResult, bool> Assigned =
			new SpecificationAdapter<TResult>(AssignedSpecification<TResult>.Default).Get;

		public HasResult(Func<TParameter, TResult> source) : this(source, Assigned) {}

		public HasResult(Func<TParameter, TResult> source, Func<TResult, bool> result) : base(result.In(source).Get) {}
	}
}