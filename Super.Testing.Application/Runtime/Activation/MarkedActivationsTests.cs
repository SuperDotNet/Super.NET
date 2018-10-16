using FluentAssertions;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Runtime.Activation
{
	public class MarkedActivationsTests
	{
		sealed class StartValues : ISelect<string, int>
		{
			public static StartValues Default { get; } = new StartValues();

			StartValues() {}

			public int Get(string parameter) => parameter.Length;
		}

		sealed class StartReferences : ISelect<int, string>
		{
			public static StartReferences Default { get; } = new StartReferences();

			StartReferences() {}

			public string Get(int parameter) => "Hello World";
		}

		sealed class Values : ISelect<Type, int>, IActivateMarker<int>
		{
			readonly int _number;

			public Values(int number) => _number = number;

			public int Get(Type parameter) => _number + parameter.AssemblyQualifiedName.Length;
		}

		sealed class References : ISelect<int, Type>, IActivateMarker<string>
		{
			readonly string _message;

			public References(string message) => _message = message;

			public Type Get(int parameter) => GetType();
		}

		[Fact]
		public void VerifyReferences()
		{
			var source = StartReferences.Default.Select(I<References>.Default);
			source.Get(123).Should().BeSameAs(source.Get(123));
		}

		[Fact]
		public void VerifyValues()
		{
			var          source    = StartValues.Default.Select(I<Values>.Default);
			const string parameter = "First";
			source.Get(parameter).Should().NotBeSameAs(source.Get(parameter));
		}
	}
}