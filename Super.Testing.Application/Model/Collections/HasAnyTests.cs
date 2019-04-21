using FluentAssertions;
using Super.Model.Collections;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public class HasAnyTests
	{
		[Fact]
		public void Has() => HasAny.Default.Get(new[] {new object()})
		                                          .Should()
		                                          .BeTrue();

		[Fact]
		public void HasNot() => HasAny.Default.Get(Empty<object>.Array)
		                                             .Should()
		                                             .BeFalse();
	}
}