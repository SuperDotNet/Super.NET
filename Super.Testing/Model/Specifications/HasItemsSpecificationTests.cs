using FluentAssertions;
using Super.Model.Specifications;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class HasItemsSpecificationTests
	{
		[Fact]
		public void Has() => HasItemsSpecification.Default.IsSatisfiedBy(new[] {new object()})
		                                          .Should()
		                                          .BeTrue();

		[Fact]
		public void HasNot() => HasItemsSpecification.Default.IsSatisfiedBy(Empty<object>.Array)
		                                          .Should()
		                                          .BeFalse();

	}
}