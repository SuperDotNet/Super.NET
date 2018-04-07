using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class FixedResult<TParameter, TResult> : ISource<TParameter, TResult>, IActivateMarker<TResult>
	{
		readonly TResult _instance;

		public FixedResult(TResult instance) => _instance = instance;

		public TResult Get(TParameter parameter) => _instance;
	}
}