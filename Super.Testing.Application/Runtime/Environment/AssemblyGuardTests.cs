using FluentAssertions;
using Super.Compose;
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
			Start.A.Result<Assembly>()
			     .By.Calling(() => null)
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyGuard()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null)
			     .ToSelect()
			     .Select(PrimaryAssemblyMessage.Default.AsGuard())
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}

		[Fact]
		void VerifyAssigned()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => GetType().Assembly)
			     .ToSelect()
			     .Select(PrimaryAssemblyMessage.Default.AsGuard())
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}
	}
}