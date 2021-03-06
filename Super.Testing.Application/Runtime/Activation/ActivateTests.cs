﻿using FluentAssertions;
using Super.Application.Hosting.xUnit;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime;
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

		sealed class Subject<TIn, TOut> : ISelect<TIn, TOut>
		{
			public static Subject<TIn, TOut> Default { get; } = new Subject<TIn, TOut>();

			Subject() {}

			public TOut Get(TIn parameter) => default;
		}

		sealed class Subject : IActivateUsing<int>
		{
			public Subject(int number) => Number = number;

			public int Number { get; }
		}

		sealed class Object : IActivateUsing<object>
		{
			public Object(object @object) => O = @object;

			public object O { get; }
		}

		[Fact]
		void VerifyGet()
		{
			IsAssigned<Singleton>.Default.Get(Singleton.Default).Should().BeTrue();

			Activator<Singleton>.Default.Get().Should().BeSameAs(Singleton.Default);

			Activator<Singleton>.Default.Get().Should().BeSameAs(Activator<Singleton>.Default.Get());
		}

		[Fact]
		void VerifyNew()
		{
			New<Activated>.Default.Get().Should().NotBeSameAs(New<Activated>.Default.Get());
		}
	}
}