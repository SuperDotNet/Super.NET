using FluentAssertions;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class DelegatesTests
	{
		[Fact]
		public void Coverage()
		{
			Delegates.Empty.Should()
			         .BeSameAs(Delegates.Empty);
			Delegates.Empty();
		}

		[Fact]
		public void CoverageGeneric()
		{
			Delegates<object>.Empty.Should()
			                 .BeSameAs(Delegates<object>.Empty);
			Delegates<object>.Empty(null);
		}
	}
}