using Super.Runtime.Activation;

namespace Super.Model.Selection.Stores
{
	public class ActivatedStore<TIn, TOut> : Store<TIn, TOut>
	{
		protected ActivatedStore() : base(New<TIn, TOut>.Default.ToDelegateReference()) {}
	}
}