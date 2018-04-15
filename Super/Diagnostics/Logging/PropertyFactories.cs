using Serilog.Core;
using Serilog.Events;
using Super.Model.Selection.Stores;

namespace Super.Diagnostics.Logging
{
	sealed class PropertyFactories : DecoratedTable<LogEvent, ILogEventPropertyFactory>
	{
		public static PropertyFactories Default { get; } = new PropertyFactories();

		PropertyFactories() : base(ReferenceTables<LogEvent, ILogEventPropertyFactory>.Default.Get(x => null)) {}
	}
}