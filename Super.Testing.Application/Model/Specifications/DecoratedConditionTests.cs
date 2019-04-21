using FluentAssertions;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class DecoratedConditionTests
	{
		[Fact]
		public void Verify()
		{
			new DecoratedCondition<object>(Always<object>.Default).Get(new object()).Should().BeTrue();
		}
	}
}