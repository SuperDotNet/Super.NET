using FluentAssertions;
using Super.Aspects;
using Super.Compose;
using System;
using Xunit;

namespace Super.Testing.Application.Aspects
{
	public class AssignedAspectTests
	{
		[Fact]
		void Verify()
		{
			var subject = Start.A.Selection<string>().By.Self;
			subject.Invoking(x => x.Get(null)).Should().NotThrow();
			AssignedAspect<string, string>.Default.Get(subject)
			                              .Invoking(x => x.Get(null))
			                              .Should()
			                              .Throw<InvalidOperationException>();
		}
	}
}