using FluentAssertions;
using Super.Runtime.Environment;
using Super.Testing.Objects;
using Super.Testing.Platform;
using Xunit;

namespace Super.Testing.Application.Model.Selection.Structure
{
	public class SelectionsTests
	{
		[Fact]
		void VerifyType()
		{
			typeof(IService<>).IsAssignableFrom(typeof(Service<>)).Should().BeFalse();
			typeof(IService<MyStruct>).IsAssignableFrom(typeof(Service<MyStruct>))
			                             .Should()
			                             .BeTrue();
		}

		[Fact]
		void VerifySelections()
		{
			Selections.Default.Get(typeof(IService<MyStruct>))
			          .Get(typeof(Service<>))
			          .Should()
			          .Be(typeof(Service<MyStruct>));
		}

		readonly struct MyStruct {}
	}
}