using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class WhereTests
	{
		[Fact]
		public void Coverage()
		{
			Where<object>.Assigned(null)
			             .Should()
			             .BeFalse();
		}
	}
}