using System;
using Serilog.Events;
using Super.ExtensionMethods;

namespace Super.Diagnostics.Logging
{
	public static class Implementations
	{
		public static Func<LogEvent, IScalar> Scalars { get; } = Logging.Scalars.Default.ToStore().ToDelegate();

		public static Func<LogEvent, LogEvent> Projections { get; }
			= ProjectionLogEvents.Default.ToStore().ToDelegate();
	}
}