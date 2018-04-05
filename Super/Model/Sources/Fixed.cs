using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class Fixed<TParameter, TResult> : ISource<TParameter, TResult>, IActivateMarker<TResult>
	{
		readonly TResult _instance;

		public Fixed(TResult instance) => _instance = instance;

		public TResult Get(TParameter parameter) => _instance;
	}
}