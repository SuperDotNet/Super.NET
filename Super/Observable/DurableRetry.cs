using Polly;
using Super.Diagnostics;
using Super.ExtensionMethods;

namespace Super.Observable
{
	public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILog logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Fix(policy).Get()) {}

		public DurableRetry(ILog logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException(logger).Execute).Fix(policy).Get()) {}
	}
}