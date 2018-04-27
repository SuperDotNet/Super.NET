using Polly;
using Serilog;
using Serilog.Core;
using Super.Diagnostics.Logging;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Commands;
using Super.Model.Selection.Alterations;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

// ReSharper disable TooManyArguments

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static T Load<T>(this ILogger @this) where T : IActivateMarker<ILogger>
			=> @this.To(I<T>.Default);


		public static void Execute<T>(this ICommand<ExceptionParameter<TimeSpan>> @this, DelegateResult<T> result,
		                              TimeSpan span, Context context)
			=> @this.Execute(new ExceptionParameter<TimeSpan>(result.Exception, span));

		public static IAlteration<LoggerConfiguration> ToConfiguration(this ILoggingSinkConfiguration @this)
			=> LoggerSinkSelector.Default.Select(@this).ToAlteration();

		public static IAlteration<LoggerConfiguration> ToConfiguration(this ILoggingEnrichmentConfiguration @this)
			=> LoggerEnrichmentSelector.Default.Select(@this).ToAlteration();

		public static IAlteration<LoggerConfiguration> ToConfiguration(this ILoggingDestructureConfiguration @this)
			=> LoggerDestructureSelector.Default.Select(@this).ToAlteration();

		public static ILoggingConfiguration WithProjections(this ILoggingSinkConfiguration @this)
			=> @this.To(I<ProjectionAwareSinkDecoration>.Default);

		public static ILoggingConfiguration WithProjections(this ILogEventSink @this)
			=> @this.To(I<ProjectionAwareSinkDecoration>.Default);
	}
}