using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class DelegatesTests
	{
		[Fact]
		public void Coverage()
		{
			Delegates.Empty.Should()
			         .BeSameAs(Delegates.Empty);
			Delegates.Empty.Invoke();
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