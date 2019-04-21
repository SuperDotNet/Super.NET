using Super.Runtime.Activation;

namespace Super.Model.Selection
{
	public class FixedResult<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<TOut>
	{
		readonly TOut _instance;

		public FixedResult(TOut instance) => _instance = instance;

		public TOut Get(TIn _) => _instance;
	}
}