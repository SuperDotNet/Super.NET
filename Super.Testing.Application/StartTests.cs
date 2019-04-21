using FluentAssertions;
using Super.Compose;
using Xunit;

namespace Super.Testing.Application
{
	public sealed class StartTests
	{
		[Fact]
		void VerifySubject()
		{
			var source = Start.A.Result<Subject>().By.Activation();
			var first  = source.Get();
			first.Should().NotBeNull();
			source.Get().Should().NotBeNull().And.Subject.Should().NotBeSameAs(first);
		}

		[Fact]
		void VerifySingleton()
		{
			var source = Start.A.Result<SingletonSubject>().By.Activation();
			var first  = source.Get();
			first.Should().Be(SingletonSubject.Default);
			source.Get().Should().BeSameAs(first);
		}

		[Fact]
		void VerifyDefault()
		{
			Start.A.Result<Subject>()
			     .By.Default()
			     .Get()
			     .Should()
			     .BeNull();
		}

		[Fact]
		void VerifyArray()
		{
			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(0)
			     .Get()
			     .Should()
			     .BeEmpty();

			Start.A.Result<Subject>()
			     .As.Sequence.Array.New(4)
			     .Get()
			     .Should()
			     .HaveCount(4);
		}

		sealed class Subject {}

		sealed class SingletonSubject
		{
			public static SingletonSubject Default { get; } = new SingletonSubject();

			SingletonSubject() {}
		}
	}
}