using Polly;
using Super.Diagnostics;
using Super.ExtensionMethods;

namespace Super.Runtime.Invocation
{
	public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILog logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Select(policy).Get()) {}

		public DurableRetry(ILog logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException(logger).Execute).Select(policy).Get()) {}
	}
}