using Serilog.Events;
using System;

namespace Super.Diagnostics.Logging
{
	public static class Implementations
	{
		public static Func<LogEvent, IScalar> Scalars { get; } = Logging.Scalars.Default.ToStore().Get;

		public static Func<LogEvent, LogEvent> Projections { get; }
			= ProjectionLogEvents.Default.ToStore().Get;
	}
}