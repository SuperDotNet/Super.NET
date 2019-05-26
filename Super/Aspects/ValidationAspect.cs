using System;
using Super.Model.Selection;
using Super.Runtime.Invocation;

namespace Super.Aspects
{
	public class ValidationAspect<TIn, TOut> : Invocation0<ISelect<TIn, TOut>, Action<TIn>, ISelect<TIn, TOut>>,
	                                           IAspect<TIn, TOut>
	{
		public ValidationAspect(Action<TIn> parameter)
			: base((select, action) => new Configured<TIn, TOut>(select, action), parameter) {}
	}
}