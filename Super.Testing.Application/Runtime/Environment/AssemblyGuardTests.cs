using FluentAssertions;
using Super.Runtime.Environment;
using System;
using System.Reflection;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class AssemblyGuardTests
	{
		[Fact]
		void Verify()
		{
			Start.New<Assembly>(() => null)
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyGuard()
		{
			Start.New<Assembly>(() => null)
			     .Select(PrimaryAssemblyMessage.Default)
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyAssigned()
		{
			Start.New(() => GetType().Assembly)
			     .Select(PrimaryAssemblyMessage.Default)
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}
	}
}