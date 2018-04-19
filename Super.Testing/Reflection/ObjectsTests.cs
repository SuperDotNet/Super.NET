using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class ObjectsTests
	{
		[Fact]
		public void AccountForNullable()
		{
			AccountForUnassignedAlteration.Default.Get(typeof(int?))
			                              .Should()
			                              .Be<int>();

			AccountForUnassignedAlteration.Default.Get(GetType())
			                              .Should()
			                              .Be(GetType());
		}
	}
}