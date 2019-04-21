using FluentAssertions;
using Super.Reflection.Collections;
using Super.Reflection.Types;
using System.Reflection;
using Super.Model.Selection.Conditions;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class InnerTypeTests
	{
		[Fact]
		public void Coverage()
		{
			InnerType.Default.Get(GetType())
			         .Should()
			         .BeNull();
		}

		[Fact]
		void Verify()
		{
			InnerType.Default.Get(Type<Always<TypeInfo>>.Instance).Should().Be<TypeInfo>();
		}
	}
}