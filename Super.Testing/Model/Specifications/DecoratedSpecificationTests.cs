using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class DecoratedSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			new DecoratedSpecification<object>(Always<object>.Default).IsSatisfiedBy(new object())
			                                                                       .Should()
			                                                                       .BeTrue();
		}
	}
}