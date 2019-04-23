using FluentAssertions;
using Super.Runtime.Environment;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class ImplementationsTests
	{
		[Fact]
		void Verify()
		{
			Implementations<object>.Store.Get().Should().NotBeSameAs(Implementations<object>.Store.Get());

			SystemStores<object>.Default.Get().Should().BeOfType<Logical<object>>();
		}
	}
}