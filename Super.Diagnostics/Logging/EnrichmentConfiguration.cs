using Serilog;
using Serilog.Configuration;
using Super.Model.Selection.Alterations;
using System;

namespace Super.Diagnostics.Logging
{
	class EnrichmentConfiguration : IAlteration<LoggerConfiguration>
	{
		readonly Func<LoggerEnrichmentConfiguration, LoggerConfiguration> _select;

		public EnrichmentConfiguration(Func<LoggerEnrichmentConfiguration, LoggerConfiguration> select)
			=> _select = select;

		public LoggerConfiguration Get(LoggerConfiguration parameter)
		{
			_select(parameter.Enrich);
			return parameter;
		}
	}
}
