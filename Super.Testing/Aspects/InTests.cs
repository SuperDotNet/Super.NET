﻿using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Selection;
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
			sut.Source(x => x).Should().BeSameAs(I<Subject, int, int>.Default);
		}

		sealed class Subject : ISelect<int, int>
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get(int parameter) => 0;
		}
	}
}