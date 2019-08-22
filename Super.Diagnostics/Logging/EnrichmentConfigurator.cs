using Serilog;
using Serilog.Configuration;
using Super.Model.Selection.Alterations;
using System;

namespace Super.Diagnostics.Logging
{
	sealed class EnrichmentConfigurator : IAlteration<LoggerConfiguration>
	{
		readonly Func<LoggerEnrichmentConfiguration, LoggerConfiguration> _select;

		public EnrichmentConfigurator(Func<LoggerEnrichmentConfiguration, LoggerConfiguration> select)
			=> _select = select;

		public LoggerConfiguration Get(LoggerConfiguration parameter)
		{
			_select(parameter.Enrich);
			return parameter;
		}
	}
}
