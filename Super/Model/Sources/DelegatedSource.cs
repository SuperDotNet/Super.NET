using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class DelegatedSource<T> : ISource<T>, IActivateMarker<Func<T>>
	{
		readonly Func<T> _source;

		public DelegatedSource(Func<T> source) => _source = source;

		public T Get() => _source();
	}
}