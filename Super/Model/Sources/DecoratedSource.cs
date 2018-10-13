using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class DecoratedSource<T> : DelegatedSource<T>, IActivateMarker<ISource<T>>
	{
		public DecoratedSource(ISource<T> source) : base(source.Get) {}
	}
}