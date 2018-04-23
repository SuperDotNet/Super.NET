﻿using FluentAssertions;
using Super.Model.Selection;
using Xunit;

namespace Super.Testing.Application.Model.Selection
{
	public sealed class DelegatesTests
	{
		[Fact]
		void Verify()
		{
			var first = Delegates<object, int>.Default.Get(Counter.Default);
			first.Should().BeSameAs(Delegates<object, int>.Default.Get(Counter.Default));

			var o = new object();
			first(o);
		}

		sealed class Counter : ISelect<object, int>
		{
			public static Counter Default { get; } = new Counter();

			Counter() {}

			int _count;

			public int Get(object parameter) => _count++;
		}
	}
}
