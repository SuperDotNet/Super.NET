using System;
using Serilog;
using Super.ExtensionMethods;

namespace Super.Diagnostics
{
	public static class Log
	{
		public static ILogger For<T>() => Logger.Default.Get().ForContext<T>();

		public static Action Dispose() => Logger.Default.Get()
		                                        .To<IDisposable>().Dispose;
	}
}