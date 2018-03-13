using System;
using System.Reactive.Linq;
using Polly;
using Super.Model.Sources;

namespace Super.Observable
{
	public class DurableObservableSource<T> : ISource<IObservable<T>, T>
	{
		readonly Func<Func<T>, T> _policy;

		public DurableObservableSource(ISyncPolicy policy) : this(policy.Execute) {}

		public DurableObservableSource(ISyncPolicy<T> policy) : this(policy.Execute) {}

		public DurableObservableSource(Func<Func<T>, T> policy) => _policy = policy;

		public T Get(IObservable<T> parameter) => _policy(parameter.Wait);
	}
}