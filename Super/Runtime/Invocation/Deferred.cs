using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	class Deferred<TParameter, TResult> : DecoratedSelect<TParameter, TResult>,
	                                      IActivateMarker<ISelect<TParameter, TResult>>
	{
		public Deferred(ISelect<TParameter, TResult> select)
			: this(@select, Tables<TParameter, TResult>.Default.Get(_ => default)) {}

		public Deferred(ISelect<TParameter, TResult> select, IMutable<TParameter, TResult> mutable)
			: base(mutable.Or(new Configuration<TParameter, TResult>(@select, mutable))) {}
	}
}