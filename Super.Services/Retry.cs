using Super.Diagnostics;
using Super.Runtime.Invocation;

namespace Super.Services
{
	sealed class Retry<T> : DurableRetry<T>
	{
		public static Retry<T> Default { get; } = new Retry<T>();

		Retry() : base(Log<Request<T>>.Default, DefaultPolicies.Default) {}
	}
}