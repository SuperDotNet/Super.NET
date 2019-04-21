using FluentAssertions;
using Moq;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class NeverTests
	{
		[Fact]
		public void Coverage()
		{
			Never.Default.Get(It.IsAny<object>())
			     .Should()
			     .BeFalse();
		}
	}
}