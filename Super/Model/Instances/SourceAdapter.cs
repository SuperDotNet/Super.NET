using System.Reactive;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Instances
{
	sealed class SourceAdapter<T> : FixedDelegatedSource<Unit, T>, IActivateMarker<IInstance<T>>
	{
		public SourceAdapter(IInstance<T> instance) : base(instance.Get) {}
	}
}