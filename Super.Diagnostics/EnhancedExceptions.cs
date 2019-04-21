using Super.Model.Selection.Stores;
using System;
using System.Diagnostics;

namespace Super.Diagnostics
{
	sealed class EnhancedExceptions : DecoratedTable<Exception, Exception>
	{
		public static EnhancedExceptions Default { get; } = new EnhancedExceptions();

		EnhancedExceptions() : base(ReferenceTables<Exception, Exception>.Default.Get(x => x.Demystify())) {}
	}

/*public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILogger logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Get(policy)) {}

		public DurableRetry(ILogger logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException(logger).Execute).Get(policy)) {}
	}*/

	/*sealed class DomainUserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static DomainUserNameEnricher Default { get; } = new DomainUserNameEnricher();

		DomainUserNameEnricher() : base(x => x.WithEnvironmentUserName()) {}
	}

	sealed class UserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static UserNameEnricher Default { get; } = new UserNameEnricher();

		UserNameEnricher() : base(x => x.WithUserName()) {}
	}

	sealed class EnhancedExceptionStackTraceConfiguration : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static EnhancedExceptionStackTraceConfiguration Default { get; } = new EnhancedExceptionStackTraceConfiguration();

		EnhancedExceptionStackTraceConfiguration() : base(x => x.WithDemystifiedStackTraces()) {}
	}*/

	/*sealed class CorrelationIdEnricher : Delegated<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static CorrelationIdEnricher Default { get; } = new CorrelationIdEnricher();

		CorrelationIdEnricher() : base(x => x.WithCorrelationId()) {}
	}

	sealed class CorrelationHeaderEnricher : ILoggingEnrichmentConfiguration
	{
		readonly string _header;

		public CorrelationHeaderEnricher(string name) => _header = name;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.WithCorrelationIdHeader(_header);
	}*/

	/*sealed class ExceptionHashEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ExceptionHashEnricher Default { get; } = new ExceptionHashEnricher();

		ExceptionHashEnricher() : base(x => x.WithExceptionStackTraceHash()) {}
	}

	sealed class MemoryEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MemoryEnricher Default { get; } = new MemoryEnricher();

		MemoryEnricher() : base(x => x.WithMemoryUsage()) {}
	}

	sealed class ProcessIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessIdEnricher Default { get; } = new ProcessIdEnricher();

		ProcessIdEnricher() : base(x => x.WithProcessId()) {}
	}

	sealed class ProcessNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessNameEnricher Default { get; } = new ProcessNameEnricher();

		ProcessNameEnricher() : base(x => x.WithProcessName()) {}
	}

	sealed class ThreadEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ThreadEnricher Default { get; } = new ThreadEnricher();

		ThreadEnricher() : base(x => x.WithThreadId()) {}
	}

	sealed class MachineNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MachineNameEnricher Default { get; } = new MachineNameEnricher();

		MachineNameEnricher() : base(ContextLoggerConfigurationExtension.WithMachineName) {}
	}

	sealed class EnvironmentVariableEnricher : ILoggingEnrichmentConfiguration
	{
		readonly string _name;

		public EnvironmentVariableEnricher(string name) => _name = name;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.WithEnvironment(_name);
	}*/
}
