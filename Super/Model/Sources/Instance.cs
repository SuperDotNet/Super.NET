using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class Instance<TParameter, TResult> : ISource<TParameter, TResult>, IActivateMarker<TResult>
	{
		readonly TResult _instance;

		public Instance(TResult instance) => _instance = instance;

		public TResult Get(TParameter parameter) => _instance;
	}
}