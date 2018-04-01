using System;
using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class InstanceOfTypeSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			IsTypeSpecification<int>.Default.IsSatisfiedBy(6776)
			                        .Should()
			                        .BeTrue();

			IsTypeSpecification<int>.Default.IsSatisfiedBy(DateTime.Now)
			                        .Should()
			                        .BeFalse();
		}
	}
}