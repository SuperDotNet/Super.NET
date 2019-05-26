using FluentAssertions;
using Super.Model.Sequences.Collections;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Collections
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