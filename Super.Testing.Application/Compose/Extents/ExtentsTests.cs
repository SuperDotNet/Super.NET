using FluentAssertions;
using Super.Compose;
using Xunit;

namespace Super.Testing.Application.Compose.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifyCondition()
		{
			var parameter = new object();
			Start.An.Extent.Of.Any.Into.Condition.Always
			     .Get(parameter)
			     .Should()
			     .BeTrue();
		}
	}
}