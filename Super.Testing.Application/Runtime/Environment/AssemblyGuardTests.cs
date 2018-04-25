using FluentAssertions;
using Super.Runtime.Environment;
using System;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class AssemblyGuardTests
	{
		[Fact]
		void Verify()
		{
			AssemblyGuard.Default.Invoking(x => x.Get(null))
			             .Should()
			             .Throw<InvalidOperationException>();
		}
	}
}