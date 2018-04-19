using Polly;
using Serilog;
using Super.Diagnostics;
using Super.Diagnostics.Logging;
using Super.Runtime.Invocation.Operations;

namespace Super.Runtime.Invocation
{
	public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILogger logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Select(policy).Get()) {}

		public DurableRetry(ILogger logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException(logger).Execute).Select(policy).Get()) {}
	}
}