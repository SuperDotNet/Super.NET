using FluentAssertions;
using Super.Diagnostics.Logging.Configuration;
using Xunit;

namespace Super.Testing.Diagnostics
{
	public class LoggingConfigurationTests
	{
		[Fact]
		void Verify()
		{
			LoggingConfiguration.Default.Get().Should().BeSameAs(DefaultLoggingConfiguration.Default);
		}
	}
}