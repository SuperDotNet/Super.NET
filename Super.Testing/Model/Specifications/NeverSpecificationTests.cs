using FluentAssertions;
using Moq;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class NeverSpecificationTests
	{
		[Fact]
		public void Coverage()
		{
			NeverSpecification.Default.IsSatisfiedBy(It.IsAny<object>())
			                  .Should()
			                  .BeFalse();
		}
	}
}