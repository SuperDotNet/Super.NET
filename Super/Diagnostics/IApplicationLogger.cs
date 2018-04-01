using System;
using Serilog;

namespace Super.Diagnostics
{
	public interface IApplicationLogger : ILogger, IDisposable {}
}