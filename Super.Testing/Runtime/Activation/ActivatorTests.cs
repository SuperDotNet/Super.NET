using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class ActivatorTests
	{
		[Theory, Framework.AutoData]
		void Verify(Activator<Subject> sut)
		{
			sut.Get().Should().BeSameAs(Subject.Default);
		}

		[Theory, Framework.AutoData]
		void VerifyNew(Activator<New> sut)
		{
			sut.Get().Should().NotBeSameAs(sut.Get());
		}

		[Theory, Framework.AutoData]
		void VerifyMoq(Mock<IActivator<New>> sut)
		{
			sut.Object.Get().Should().NotBeNull();
		}

		[Theory, AutoData]
		void VerifyNativeMoq(Mock<IActivator<New>> sut)
		{
			sut.Object.Get().Should().BeNull();
		}
	}

	sealed class Subject
	{
		public static Subject Default { get; } = new Subject();

		Subject() {}
	}

	sealed class New {}
}