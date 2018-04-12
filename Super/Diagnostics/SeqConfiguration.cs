using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Super.ExtensionMethods;
using System;

namespace Super.Diagnostics
{
	public class SeqConfiguration : ILoggingSinkConfiguration
	{
		readonly Uri _uri;
		readonly Func<LoggingLevelSwitch> _switch;

		public SeqConfiguration(Uri uri) : this(uri, LoggingLevelController.Default.ToDelegate()) {}

		public SeqConfiguration(Uri uri, Func<LoggingLevelSwitch> @switch)
		{
			_uri = uri;
			_switch = @switch;
		}

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter) => parameter.Seq(_uri.ToString(), controlLevelSwitch: _switch());
	}
}