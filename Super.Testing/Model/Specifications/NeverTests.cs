using FluentAssertions;
using Moq;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class NeverTests
	{
		[Fact]
		public void Coverage()
		{
			Never.Default.IsSatisfiedBy(It.IsAny<object>())
			                  .Should()
			                  .BeFalse();
		}
	}
}