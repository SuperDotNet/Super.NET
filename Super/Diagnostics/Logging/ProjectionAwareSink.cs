using System;
using Serilog.Core;
using Serilog.Events;
using Super.Model.Commands;
using Super.Runtime.Activation;

namespace Super.Diagnostics.Logging
{
	sealed class ProjectionAwareSink : SelectedParameterCommand<LogEvent, LogEvent>,
	                                   IDisposable,
	                                   ILogEventSink,
	                                   IActivateUsing<ILogEventSink>
	{
		readonly IDisposable _disposable;

		public ProjectionAwareSink(ILogEventSink sink) : this(sink, sink.ToDisposable()) {}

		public ProjectionAwareSink(ILogEventSink sink, IDisposable disposable)
			: base(sink.Emit, Implementations.Projections) => _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}

		public void Emit(LogEvent logEvent)
		{
			Execute(logEvent);
		}
	}
}