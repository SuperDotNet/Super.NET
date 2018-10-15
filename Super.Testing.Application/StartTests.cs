using Xunit;

// ReSharper disable All

namespace Super.Testing.Application
{
	public sealed class StartTests
	{
		[Fact]
		void Verify()
		{
			Start.From<object>().Self().Select(x => x.ToString());
		}
	}
}