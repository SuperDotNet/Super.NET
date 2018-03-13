using FluentAssertions;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Testing.Framework;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Runtime.Activation
{
	public sealed class ActivateTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var subject = I<Subject>.Default.New(number);
			subject.Should().NotBeSameAs(I<Subject>.Default.New(number));

			subject.Number.Should().Be(number);
		}

		[Theory, AutoData]
		void VerifyObject(I<Object> sut)
		{
			var first = new object();
			var second = new object();
			sut.New(first).Should().NotBeSameAs(sut.New(first));
		}

		[Theory, AutoFixture.Xunit2.AutoData]
		void VerifyActivateExtensionMethod(Subject<string, string> sut)
		{
			sut.Should().BeSameAs(Subject<string, string>.Default);
		}

		sealed class Subject<TParameter, TResult> : ISource<TParameter, TResult>
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
	}
}