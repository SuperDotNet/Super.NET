using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation
{
	sealed class Striped<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		public Striped(ISelect<TIn, TOut> select) : base(select.To(I.A<Deferred<TIn, TOut>>())
		                                                        .ToDelegate()
		                                                        .To(I.A<Stripe<TIn, TOut>>())
		                                                        .Unless(select)) {}
	}
}