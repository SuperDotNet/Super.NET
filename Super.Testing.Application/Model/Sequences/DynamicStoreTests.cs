﻿using FluentAssertions;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public class DynamicStoreTests
	{
		[Theory]
		[InlineData(1u)]
		[InlineData(2u)]
		[InlineData(3u)]
		[InlineData(4u)]
		[InlineData(5u)]
		[InlineData(1_000u)]
		[InlineData(1_025u)]
		[InlineData(10_000u)]
		[InlineData(100_000u)]
		[InlineData(1_000_000u)]
		void Verify(uint count)
		{
			var source = Enumerable.Range(0, (int)count).Select(x => (uint)x).ToArray();
			DynamicIterator<uint>.Default.Get(new Iteration<uint>(source)).Should().Equal(source);
		}
	}
}