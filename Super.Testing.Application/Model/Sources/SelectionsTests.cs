﻿using FluentAssertions;
using Super.Model.Selection;
using Super.Runtime.Activation;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Model.Sources
{
	public sealed class SelectionsTests
	{
		sealed class Count : IActivateUsing<int>
		{
			public static int Counter = 0;

			public Count(int number)
			{
				Number = number;
				Counter++;
			}

			public int Number { get; }
		}

		sealed class Other : IActivateUsing<int>
		{
			public static int Count = 0;

			public Other(int number)
			{
				Number = number;
				Count++;
			}

			public int Number { get; }
		}

		sealed class Subject<TIn, TOut> : ISelect<TIn, TOut>
		{
			public static Subject<TIn, TOut> Default { get; } = new Subject<TIn, TOut>();

			Subject() {}

			public TOut Get(TIn parameter) => default;
		}

		[Fact]
		public void Verify()
		{
			var subject = Subject<string, int>.Default.ToDelegateReference();
			subject.Should()
			       .BeSameAs(Subject<string, int>.Default.ToDelegateReference());

			subject.ToSelect().Should().BeSameAs(subject.ToSelect());
		}
	}
}