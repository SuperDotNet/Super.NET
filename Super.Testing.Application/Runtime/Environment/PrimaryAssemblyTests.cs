using FluentAssertions;
using Super.Runtime.Environment;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class PrimaryAssemblyTests
	{
		[Fact]
		void Verify()
		{
			PrimaryAssembly.Default.Get().Should().BeSameAs(GetType().Assembly);
		}
	}
}