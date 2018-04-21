using Serilog.Core;
using Serilog.Events;
using Super.Model.Selection.Stores;

namespace Super.Diagnostics.Logging
{
	sealed class PropertyFactories : ReferenceValueTable<LogEvent, ILogEventPropertyFactory>
	{
		public static PropertyFactories Default { get; } = new PropertyFactories();

		PropertyFactories() {}
	}
}