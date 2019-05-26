using FluentAssertions;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class ObjectsTests
	{
		[Fact]
		public void AccountForNullable()
		{
			AccountForUnassignedType.Default.Get(typeof(int?))
			                        .Should()
			                        .Be<int>();

			AccountForUnassignedType.Default.Get(GetType())
			                        .Should()
			                        .Be(GetType());
		}
	}
}