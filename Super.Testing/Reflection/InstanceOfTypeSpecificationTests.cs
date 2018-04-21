using System;
using FluentAssertions;
using Super.Reflection.Types;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class InstanceOfTypeSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			IsType<int>.Default.IsSatisfiedBy(6776)
			                        .Should()
			                        .BeTrue();

			IsType<int>.Default.IsSatisfiedBy(DateTime.Now)
			                        .Should()
			                        .BeFalse();
		}
	}
}