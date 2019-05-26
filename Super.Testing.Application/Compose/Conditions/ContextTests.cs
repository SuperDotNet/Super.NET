using FluentAssertions;
using Super.Compose;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Compose.Conditions
{
	public sealed class ContextTests
	{
		[Fact]
		void Verify()
		{
			Start
				.A.Condition.Of.Any.By.Always.Should()
				.BeSameAs(Always<object>.Default);
		}

		[Fact]
		void VerifyUsing()
		{
			var subject = Start.A.Condition.Of.Type<int>().By.Calling(x => x > 3);

			subject.Get(0).Should().BeFalse();
			subject.Get(3).Should().BeFalse();
			subject.Get(4).Should().BeTrue();
		}
	}
}