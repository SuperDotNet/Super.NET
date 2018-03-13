using FluentAssertions;
using JetBrains.Annotations;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class HasSingletonPropertyTests
	{
		[Fact]
		public void Is() => HasSingletonProperty.Default.IsSatisfiedBy(typeof(Contains))
		                                                          .Should()
		                                                          .BeTrue();

		[Fact]
		public void IsNot() => HasSingletonProperty.Default.IsSatisfiedBy(GetType())
		                                                             .Should()
		                                                             .BeFalse();

		class Contains
		{
			[UsedImplicitly]
			public static Contains Default { get; } = new Contains();
			Contains() {}
		}
	}
}