using FluentAssertions;
using Xunit;

namespace Super.Testing.Application.Compose.Selections
{
	public sealed class ContextTests
	{
		[Fact]
		void VerifyParameter()
		{
			Super.Compose
			     .Start.A.Selection.Of.Any.By.Returning(4)
			     .Get(new object())
			     .Should()
			     .Be(4);
		}

		[Fact]
		void VerifyFull()
		{
			var instance  = new Subject();
			var parameter = new object();
			var result    = new Subject();

			var subject = Super.Compose.Start.A.Selection.Of.Any.AndOf<Subject>().By.Cast.Or.Return(result);
			subject.Get(parameter).Should().BeSameAs(result);
			subject.Get(instance).Should().BeSameAs(instance);
		}

		sealed class Subject {}
	}
}