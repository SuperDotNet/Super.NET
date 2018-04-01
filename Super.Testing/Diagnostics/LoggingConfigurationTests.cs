using FluentAssertions;
using Super.Diagnostics;
using Xunit;

namespace Super.Testing.Diagnostics
{
	public class LoggingConfigurationTests
	{
		[Fact]
		void Verify()
		{
			LoggingConfiguration.Default.Get().Should().BeSameAs(DefaultLoggingConfigurations.Default);
		}
	}
}