using FluentAssertions;
using Super.Expressions;
using Xunit;

namespace Super.Testing.Expressions
{
	public class ParameterTests
	{
		[Fact]
		public void Coverage()
		{
			Parameter.Default.Should()
			         .BeSameAs(Parameter.Default);
		}
	}
}