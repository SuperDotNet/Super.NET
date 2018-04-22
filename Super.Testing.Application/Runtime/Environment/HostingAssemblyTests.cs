using FluentAssertions;
using Super.Application.Hosting.xUnit;
using Super.Runtime.Environment;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class HostingAssemblyTests
	{
		[Fact]
		void Verify()
		{
			HostingAssembly.Default.Get().Should().BeSameAs(typeof(XunitTestingApplicationAttribute).Assembly);
		}
	}
}