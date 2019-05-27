using FluentAssertions;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			new Class1().Should().NotBeNull();
		}
	}
}
