using Super.Runtime.Activation;
using System;

namespace Super.Model.Instances
{
	public class DelegatedInstance<T> : IInstance<T>, IActivateMarker<Func<T>>
	{
		readonly Func<T> _source;

		public DelegatedInstance(Func<T> source) => _source = source;

		public T Get() => _source();
	}
}