using FluentAssertions;
using Super.Application.Hosting.xUnit;
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
		public void VerifyNumber(int number)
		{
			var subject = I<Subject>.Default.From(number);
			subject.Should().NotBeSameAs(I<Subject>.Default.From(number));

			subject.Number.Should().Be(number);
		}

		[Theory, AutoData]
		void VerifyParameter(I<Object> sut, object parameter)
		{
			sut.New(parameter).Should().NotBeSameAs(sut.New(parameter));
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
			Activator<Singleton>.Default.Get().Should().BeSameAs(Singleton.Default);
		}

		[Fact]
		void VerifyNew()
		{
			New<Activated>.Default.Get().Should().NotBeSameAs(New<Activated>.Default.Get());
		}
	}
}