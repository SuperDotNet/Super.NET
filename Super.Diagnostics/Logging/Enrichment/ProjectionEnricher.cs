﻿using Serilog.Core;
using Serilog.Events;
using Super.Model.Commands;
using Super.Runtime.Objects;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class ProjectionEnricher : ILogEventEnricher
	{
		readonly ICommand<LogEvent>                          _command;
		readonly IAssign<LogEvent, ILogEventPropertyFactory> _table;

		public ProjectionEnricher(IProjectors projectors)
			: this(new ApplyProjectionsCommand(projectors), PropertyFactories.Default) {}

		public ProjectionEnricher(ICommand<LogEvent> command, IAssign<LogEvent, ILogEventPropertyFactory> table)
		{
			_command = command;
			_table   = table;
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			_command.Execute(logEvent);

			_table.Assign(logEvent, propertyFactory);
		}
	}
}