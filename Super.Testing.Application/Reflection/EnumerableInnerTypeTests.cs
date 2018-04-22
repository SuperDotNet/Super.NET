using System;
using System.Collections.Generic;
using FluentAssertions;
using Super.Reflection.Collections;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class EnumerableInnerTypeTests
	{
		[Fact]
		public void Array()
		{
			EnumerableInnerType.Default.Get(typeof(int[]))
			                   .Should()
			                   .Be<int>();
		}

		[Fact]
		public void Enumerable()
		{
			EnumerableInnerType.Default.Get(typeof(IEnumerable<DateTimeOffset>))
			                   .Should()
			                   .Be<DateTimeOffset>();
		}
	}
}