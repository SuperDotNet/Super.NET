using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class AnySpecificationTests
	{
		[Fact]
		public void Is()
		{
			AlwaysSpecification<object>.Default.Or(NeverSpecification<object>.Default)
			                           .IsSatisfiedBy(null)
			                           .Should()
			                           .BeTrue();
		}

		[Fact]
		public void IsNot()
		{
			NeverSpecification<object>.Default.Or(NeverSpecification<object>.Default)
			                           .IsSatisfiedBy(null)
			                           .Should()
			                           .BeFalse();
		}
	}
}