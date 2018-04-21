using FluentAssertions;
using Super.Application.Host.xUnit;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class FirstTests
	{
		[Theory, AutoDataModest]
		void VerifyFirst(First sut)
		{
			sut.IsSatisfiedBy().Should().BeTrue();
			sut.IsSatisfiedBy().Should().BeFalse();
		}

		[Theory, AutoData]
		void VerifyFirstReference(First<object> sut, object first, object second)
		{
			sut.IsSatisfiedBy(first).Should().BeTrue();
			sut.IsSatisfiedBy(first).Should().BeFalse();

			sut.IsSatisfiedBy(second).Should().BeTrue();
			sut.IsSatisfiedBy(second).Should().BeFalse();
		}

		[Theory, AutoData]
		void VerifyFirstEquality(First<int> sut, int first, int second)
		{
			sut.IsSatisfiedBy(first).Should().BeTrue();
			sut.IsSatisfiedBy(first).Should().BeFalse();

			sut.IsSatisfiedBy(second).Should().BeTrue();
			sut.IsSatisfiedBy(second).Should().BeFalse();
		}
	}
}