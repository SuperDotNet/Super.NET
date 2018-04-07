﻿using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Runtime.Activation
{
	public class ActivationsTests
	{
		sealed class StartValues : ISource<string, int>
		{
			public static StartValues Default { get; } = new StartValues();

			StartValues() {}

			public int Get(string parameter) => parameter.Length;
		}

		sealed class StartReferences : ISource<int, string>
		{
			public static StartReferences Default { get; } = new StartReferences();

			StartReferences() {}

			public string Get(int parameter) => "Hello World";
		}

		sealed class Values : ISource<Type, int>, IActivateMarker<int>
		{
			readonly int _number;

			public Values(int number) => _number = number;

			public int Get(Type parameter) => _number + parameter.AssemblyQualifiedName.Length;
		}

		sealed class References : ISource<int, Type>, IActivateMarker<string>
		{
			readonly string _message;

			public References(string message) => _message = message;

			public Type Get(int parameter) => GetType();
		}

		[Fact]
		public void VerifyReferences()
		{
			var source = StartReferences.Default.Out(New<References>.Default);
			source.Get(123).Should().BeSameAs(source.Get(123));
		}

		[Fact]
		public void VerifyValues()
		{
			var          source    = StartValues.Default.Out(New<Values>.Default);
			const string parameter = "First";
			source.Get(parameter).Should().NotBeSameAs(source.Get(parameter));
		}
	}
}