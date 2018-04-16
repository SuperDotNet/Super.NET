using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	sealed class Striped<TParameter, TResult> : Decorated<TParameter, TResult>,
	                                            IActivateMarker<ISelect<TParameter, TResult>>
	{
		public Striped(ISelect<TParameter, TResult> @select)
			: base(select.Or(select.New(I<Deferred<TParameter, TResult>>.Default)
			                       .ToDelegate()
			                       .To(I<Stripe<TParameter, TResult>>.Default))) {}
	}
}