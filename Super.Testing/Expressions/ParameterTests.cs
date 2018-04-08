using FluentAssertions;
using Super.Runtime.Invocation.Expressions;
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