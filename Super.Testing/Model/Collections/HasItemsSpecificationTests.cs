using FluentAssertions;
using Super.Model.Collections;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Model.Collections
{
	public class HasAnyTests
	{
		[Fact]
		public void Has() => HasAny.Default.IsSatisfiedBy(new[] {new object()})
		                                          .Should()
		                                          .BeTrue();

		[Fact]
		public void HasNot() => HasAny.Default.IsSatisfiedBy(Empty<object>.Array)
		                                             .Should()
		                                             .BeFalse();
	}
}