﻿using System;
using FluentAssertions;
using Super.Model.Selection;
using Super.Runtime.Activation;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Runtime.Activation
{
	public class ActivationsTests
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

		sealed class Values : ISelect<Type, int>, IActivateUsing<int>
		{
			readonly int _number;

			public Values(int number) => _number = number;

			public int Get(Type parameter) => _number + parameter.AssemblyQualifiedName.Length;
		}

		sealed class References : ISelect<int, Type>, IActivateUsing<string>
		{
			readonly string _message;

			public References(string message) => _message = message;

			public Type Get(int parameter) => GetType();
		}

		[Fact]
		public void VerifyReferences()
		{
			var source = StartReferences.Default.Then().Activate<References>().Get();
			source.Get(123).Should().BeSameAs(source.Get(123));
		}

		[Fact]
		public void VerifyValues()
		{
			var          source    = StartValues.Default.Then().Activate<Values>().Get();
			const string parameter = "First";
			source.Get(parameter).Should().NotBeSameAs(source.Get(parameter));
		}
	}
}