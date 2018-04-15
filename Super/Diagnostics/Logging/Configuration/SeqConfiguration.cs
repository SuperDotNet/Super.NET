using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Super.ExtensionMethods;

namespace Super.Diagnostics.Logging.Configuration
{
	public class SeqConfiguration : ILoggingSinkConfiguration
	{
		readonly Func<LoggingLevelSwitch> _switch;
		readonly Uri                      _uri;

		public SeqConfiguration(Uri uri) : this(uri, LoggingLevelController.Default.ToDelegate()) {}

		public SeqConfiguration(Uri uri, Func<LoggingLevelSwitch> @switch)
		{
			_uri    = uri;
			_switch = @switch;
		}

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter)
			=> parameter.Seq(_uri.ToString(), controlLevelSwitch: _switch());
	}
}