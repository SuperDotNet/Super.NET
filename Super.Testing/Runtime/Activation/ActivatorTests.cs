using FluentAssertions;
using Moq;
using Super.Runtime.Activation;
using Super.Testing.Framework;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class ActivatorTests
	{
		[Theory]
		[AutoData]
		void Verify(Activator<Subject> sut)
		{
			sut.Get().Should().BeSameAs(Subject.Default);
		}

		[Theory]
		[AutoData]
		void VerifyNew(Activator<New> sut)
		{
			sut.Get().Should().NotBeSameAs(sut.Get());
		}

		[Theory]
		[AutoData]
		void VerifyMoq(Mock<IActivator<New>> sut)
		{
			sut.Object.Get().Should().NotBeNull();
		}

		[Theory]
		[AutoFixture.Xunit2.AutoData]
		void VerifyNativeMoq(Mock<IActivator<New>> sut)
		{
			sut.Object.Get().Should().BeNull();
		}
	}
}