using FluentAssertions;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Compose.Conditions
{
	public sealed class ContextTests
	{
		[Fact]
		void Verify()
		{
			Super.Compose.Start
			     .A.Condition.Of.Any.By.Always.Should()
			     .BeSameAs(Always<object>.Default);
		}

		[Fact]
		void VerifyUsing()
		{
			var subject = Super.Compose.Start.A.Condition.Of.Type<int>().By.Calling(x => x > 3);

			subject.Get(0).Should().BeFalse();
			subject.Get(3).Should().BeFalse();
			subject.Get(4).Should().BeTrue();
		}
	}
}