using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class InverseSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			AlwaysSpecification<object>.Default.Inverse()
			                           .IsSatisfiedBy(new object())
			                           .Should()
			                           .BeFalse();
		}
	}
}