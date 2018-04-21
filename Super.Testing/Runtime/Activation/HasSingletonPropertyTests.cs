using FluentAssertions;
using JetBrains.Annotations;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Application.Runtime.Activation
{
	public class HasSingletonPropertyTests
	{
		class Contains
		{
			[UsedImplicitly]
			public static Contains Default { get; } = new Contains();

			Contains() {}
		}

		[Fact]
		public void Is() => HasSingletonProperty.Default.IsSatisfiedBy(typeof(Contains))
		                                        .Should()
		                                        .BeTrue();

		[Fact]
		public void IsNot() => HasSingletonProperty.Default.IsSatisfiedBy(GetType())
		                                           .Should()
		                                           .BeFalse();
	}
}