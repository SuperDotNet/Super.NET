using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	sealed class Striped<TParameter, TResult> : DecoratedSelect<TParameter, TResult>,
	                                            IActivateMarker<ISelect<TParameter, TResult>>
	{
		public Striped(ISelect<TParameter, TResult> @select)
			: base(select.To(I<Deferred<TParameter, TResult>>.Default)
			             .ToDelegate()
			             .To(I<Stripe<TParameter, TResult>>.Default)
			             .Unless(select)) {}
	}
}