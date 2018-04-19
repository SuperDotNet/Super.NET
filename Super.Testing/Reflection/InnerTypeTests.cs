using FluentAssertions;
using Super.Reflection.Collections;
using Xunit;

namespace Super.Testing.Reflection
{
	public class InnerTypeTests
	{
		[Fact]
		public void Coverage() => InnerType.Default.Get(GetType())
		                                   .Should()
		                                   .BeNull();
	}
}