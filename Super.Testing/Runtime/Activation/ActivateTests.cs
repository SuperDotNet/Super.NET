using FluentAssertions;
using Super.Application.Host.xUnit;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Runtime.Activation
{
	public sealed class ActivateTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var subject = I<Subject>.Default.From(number);
			subject.Should().NotBeSameAs(I<Subject>.Default.From(number));

			subject.Number.Should().Be(number);
		}

		[Theory, AutoData]
		void VerifyObject(I<Object> sut)
		{
			var first  = new object();
			first.New(sut).Should().NotBeSameAs(first.New(sut));
		}

		[Theory, AutoFixture.Xunit2.AutoData]
		void VerifyActivateExtensionMethod(Subject<string, string> sut)
		{
			sut.Should().BeSameAs(Subject<string, string>.Default);
		}

		sealed class Activated {}

		sealed class Singleton
		{
			public static Singleton Default { get; } = new Singleton();

			Singleton() {}
		}

		sealed class Subject<TParameter, TResult> : ISelect<TParameter, TResult>
		{
			public static Subject<TParameter, TResult> Default { get; } = new Subject<TParameter, TResult>();

			Subject() {}

			public TResult Get(TParameter parameter) => default;
		}

		sealed class Subject : IActivateMarker<int>
		{
			public Subject(int number) => Number = number;

			public int Number { get; }
		}

		sealed class Object : IActivateMarker<object>
		{
			public Object(object @object) => O = @object;

			public object O { get; }
		}

		[Fact]
		void VerifyGet()
		{
			Activate<Singleton>.Get().Should().BeSameAs(Singleton.Default);
		}

		[Fact]
		void VerifyNew()
		{
			Activate<Activated>.New().Should().NotBeSameAs(Activate<Activated>.New());
		}
	}
}