using FluentAssertions;
using Super.Runtime.Environment;
using Super.Testing.Objects;
using System;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class DefaultComponentTests
	{
		[Fact]
		void VerifyEnvironment()
		{
			RequiredComponent<IHelloWorld>.Default.Get()
			                              .GetMessage()
			                              .Should()
			                              .Be($"Hello From {PrimaryAssemblyDetails.Default.Get().Configuration}!");
		}

		[Fact]
		void VerifyInvalidEnvironment()
		{
			RequiredComponent<IInvalid>.Default.Invoking(x => x.Get()).Should().Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyPlatform()
		{
			RequiredComponent<string>.Default.Get()
			                         .Should()
			                         .Be($"Hello World from {AppContext.TargetFrameworkName}!");
		}

		interface IInvalid {}
	}
}