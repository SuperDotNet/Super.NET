using Serilog.Core;
using Super.Diagnostics.Logging.Configuration;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Diagnostics.Logging
{
	public sealed class ProjectionAwareSinkDecoration : LoggerSinkDecoration,
	                                                    IActivateMarker<ILoggingSinkConfiguration>,
	                                                    IActivateMarker<ILogEventSink>
	{
		public ProjectionAwareSinkDecoration(ILogEventSink sink) : this(new SinkConfiguration(sink)) {}

		public ProjectionAwareSinkDecoration(ILoggingSinkConfiguration configuration)
			: base(I<ProjectionAwareSink>.Default.From, configuration) {}
	}
}