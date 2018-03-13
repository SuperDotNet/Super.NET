using Polly;
using Serilog;
using Super.Diagnostics;
using Super.ExtensionMethods;

namespace Super.Observable
{
	public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILogger logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Fix(policy).Get()) {}

		public DurableRetry(ILogger logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException(logger).Execute).Fix(policy).Get()) {}
	}
}