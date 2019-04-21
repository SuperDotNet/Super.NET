using FluentAssertions;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class InverseConditionTests
	{
		[Fact]
		public void Verify()
		{
			Always<object>.Default.Then()
			              .Inverse()
			              .Get()
			              .Get(new object())
			              .Should()
			              .BeFalse();
		}
	}
}