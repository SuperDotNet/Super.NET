using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

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
		readonly IEnumerable<Type> _types;

		public ScalarConfiguration(params Type[] types) : this(types.Hide()) {}

		public ScalarConfiguration(IEnumerable<Type> types) => _types = types;

		public LoggerConfiguration Get(LoggerDestructuringConfiguration parameter) => _types.Alter(parameter.AsScalar);
	}
}