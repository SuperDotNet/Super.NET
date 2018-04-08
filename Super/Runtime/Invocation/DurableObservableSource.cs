using System;
using System.Reactive.Linq;
using Polly;
using Super.Model.Selection;

namespace Super.Runtime.Invocation
{
	public interface IObserve<T> : ISelect<IObservable<T>, T> {}

	public class DurableObservableSource<T> : IObserve<T>
	{
		readonly Func<Func<T>, T> _policy;

		public DurableObservableSource(ISyncPolicy policy) : this(policy.Execute) {}

		public DurableObservableSource(ISyncPolicy<T> policy) : this(policy.Execute) {}

		public DurableObservableSource(Func<Func<T>, T> policy) => _policy = policy;

		public T Get(IObservable<T> parameter) => _policy(parameter.Wait);
	}
}