using FluentAssertions;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Runtime
{
	public class DelegatedDisposableTests
	{
		[Fact]
		public void Verify()
		{
			var called = false;
			using (new DelegatedDisposable(() => called = true))
			{
				called.Should()
				      .BeFalse();
			}

			called.Should()
			      .BeTrue();
		}
	}
}