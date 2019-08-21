using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;

namespace Super.Diagnostics.Logging.Configuration
{
	sealed class ScalarConfiguration<T> : Select<LoggerDestructuringConfiguration, LoggerConfiguration>,
	                                      ILoggingDestructureConfiguration
	{
		public static ScalarConfiguration<T> Default { get; } = new ScalarConfiguration<T>();

		ScalarConfiguration() : base(x => x.AsScalar<T>()) {}
	}

	sealed class ScalarConfiguration : ILoggingDestructureConfiguration
	{
		readonly Array<Type> _types;

		public ScalarConfiguration(params Type[] types) : this(types.Result()) {}

		public ScalarConfiguration(Array<Type> types) => _types = types;

		public LoggerConfiguration Get(LoggerDestructuringConfiguration parameter) => _types.Reference()
		                                                                                    .Alter(parameter.AsScalar);
	}
}