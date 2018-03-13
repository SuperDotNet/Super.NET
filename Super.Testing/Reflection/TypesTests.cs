using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class TypesTests
	{
		[Fact]
		public void New()
		{
			Types<object>.New.Invoke()
			             .Should()
			             .NotBeNull();
		}
	}
}