using FluentAssertions;
using Super.Reflection.Types;
using System;
using Xunit;

namespace Super.Testing.Reflection
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