﻿using FluentAssertions;
using Super.Aspects;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection;
using Super.Testing.Framework;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Aspects
{
	public sealed class InTests
	{
		[Theory, AutoData]
		void Verify(I<Subject> sut)
		{
			sut.From(Source.Infer).Should().BeSameAs(I<ISource<int, int>>.Default);
		}

		sealed class Subject : ISource<int, int>
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get(int parameter) => 0;
		}
	}
}