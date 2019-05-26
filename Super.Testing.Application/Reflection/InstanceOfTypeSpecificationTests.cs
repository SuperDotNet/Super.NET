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
			IsOf<int>.Default.Get(6776)
			         .Should()
			         .BeTrue();

			IsOf<int>.Default.Get(DateTime.Now)
			         .Should()
			         .BeFalse();
		}
	}
}