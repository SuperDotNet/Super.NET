using System;
using System.Collections.Generic;
using FluentAssertions;
using Super.ExtensionMethods;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class EnumerableInnerTypeTests
	{
		[Fact]
		public void Enumerable()
		{
			EnumerableInnerType.Default.Get(typeof(IEnumerable<DateTimeOffset>))
			                   .Should()
			                   .Be<DateTimeOffset>();
		}

		[Fact]
		public void Array()
		{
			EnumerableInnerType.Default.Get(typeof(int[]))
			                   .Should()
			                   .Be<int>();
		}
	}
}