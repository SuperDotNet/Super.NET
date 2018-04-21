using FluentAssertions;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class AnySpecificationTests
	{
		[Fact]
		public void Is()
		{
			Always<object>.Default.Or(Never<object>.Default)
			                           .IsSatisfiedBy(null)
			                           .Should()
			                           .BeTrue();
		}

		[Fact]
		public void IsNot()
		{
			Never<object>.Default.Or(Never<object>.Default)
			                          .IsSatisfiedBy(null)
			                          .Should()
			                          .BeFalse();
		}
	}
}