using FluentAssertions;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class AnyConditionTests
	{
		[Fact]
		public void Is()
		{
			Always<object>.Default.Then()
			              .Or(Never<object>.Default)
			              .Get()
			              .Get(null)
			              .Should()
			              .BeTrue();
		}

		[Fact]
		public void IsNot()
		{
			Never<object>.Default.Then()
			             .Or(Never<object>.Default)
			             .Get()
			             .Get(null)
			             .Should()
			             .BeFalse();
		}
	}
}