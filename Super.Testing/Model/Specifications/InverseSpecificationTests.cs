using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class InverseSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			Always<object>.Default.Inverse()
			                           .IsSatisfiedBy(new object())
			                           .Should()
			                           .BeFalse();
		}
	}
}