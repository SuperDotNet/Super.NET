using System;
using Serilog;

namespace Super.Diagnostics
{
	public interface IPrimaryLogger : ILogger, IDisposable {}
}