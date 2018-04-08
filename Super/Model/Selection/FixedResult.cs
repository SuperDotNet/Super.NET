using Super.Runtime.Activation;

namespace Super.Model.Selection
{
	public class FixedResult<TParameter, TResult> : ISelect<TParameter, TResult>, IActivateMarker<TResult>
	{
		readonly TResult _instance;

		public FixedResult(TResult instance) => _instance = instance;

		public TResult Get(TParameter parameter) => _instance;
	}
}