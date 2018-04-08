using System;
using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class Delegated<T> : ISource<T>, IActivateMarker<Func<T>>
	{
		readonly Func<T> _source;

		public Delegated(Func<T> source) => _source = source;

		public T Get() => _source();
	}
}