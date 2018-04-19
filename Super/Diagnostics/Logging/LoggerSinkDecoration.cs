using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics.Logging
{
	public class LoggerSinkDecoration : ILoggingConfiguration
	{
		readonly Action<LoggerSinkConfiguration>    _configure;
		readonly Func<ILogEventSink, ILogEventSink> _sink;

		public LoggerSinkDecoration(IAlteration<ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink.ToDelegate(), configuration) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink, configuration.ToCommand().ToDelegate()) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, Action<LoggerSinkConfiguration> configure)
		{
			_sink      = sink;
			_configure = configure;
		}

		public LoggerConfiguration Get(LoggerConfiguration parameter)
			=> LoggerSinkConfiguration.Wrap(parameter.WriteTo, _sink, _configure);
	}
}