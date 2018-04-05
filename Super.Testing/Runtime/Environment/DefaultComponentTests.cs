using FluentAssertions;
using Super.Runtime.Environment;
using Super.Testing.Objects;
using System;
using Xunit;

namespace Super.Testing.Runtime.Environment
{
	public sealed class DefaultComponentTests
	{
		[Fact]
		void VerifyEnvironment()
		{
			DefaultComponent<IHelloWorld>.Default.Get()
			                   .GetMessage()
			                   .Should()
			                   .Be($"Hello From {ExecutionConfiguration.Default.Get()}!");
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