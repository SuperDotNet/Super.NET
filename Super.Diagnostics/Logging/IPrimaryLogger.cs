using System;
using Serilog;

namespace Super.Diagnostics.Logging
{
	public interface IPrimaryLogger : ILogger, IDisposable {}
}