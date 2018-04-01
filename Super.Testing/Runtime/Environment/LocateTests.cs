using FluentAssertions;
using Super.Runtime.Environment;
using Super.Testing.Objects;
using System;
using Xunit;

namespace Super.Testing.Runtime.Environment
{
	public sealed class LocateTests
	{
		[Fact]
		void VerifyEnvironment()
		{
			Locate<IHelloWorld>.Default.Get()
			                   .GetMessage()
			                   .Should()
			                   .Be($"Hello From {ExecutionConfiguration.Default.Get()}!");
		}

		[Fact]
		void VerifyInvalidEnvironment()
		{
			Locate<LocateTests>.Default.Invoking(x => x.Get()).Should().Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyPlatform()
		{
			Locate<string>.Default.Get()
			              .Should()
			              .Be($"Hello World from {AppContext.TargetFrameworkName}!");
		}
	}
}