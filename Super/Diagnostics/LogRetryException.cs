using System;
using Serilog;

namespace Super.Diagnostics
{
	public sealed class LogRetryException : LogException<TimeSpan>
	{
		public LogRetryException(ILogger logger) : this(logger.Warning) {}

		public LogRetryException(Exception<TimeSpan> action)
			: base(action, "Exception encountered during a retry-aware context.  Waiting {Wait} until next attempt...") {}
	}
}