using System;
using FluentAssertions;
using Super.Runtime.Environment;
using Super.Testing.Objects;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class DefaultComponentTests
	{
		[Fact]
		void VerifyEnvironment()
		{
			DefaultComponent<IHelloWorld>.Default.Get()
			                   .GetMessage()
			                   .Should()
			                   .Be($"Hello From {PrimaryAssemblyDetails.Default.Get().Configuration}!");
		}

		[Fact]
		void VerifyInvalidEnvironment()
		{
			DefaultComponent<DefaultComponentTests>.Default.Invoking(x => x.Get()).Should().Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyPlatform()
		{
			DefaultComponent<string>.Default.Get()
			              .Should()
			              .Be($"Hello World from {AppContext.TargetFrameworkName}!");
		}
	}
}