// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{
		readonly ITestOutputHelper _output;

		public WhereTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Size()
		{
			_output.WriteLine($"{Unsafe.SizeOf<Store<int>>()}");
		}

		[Fact]
		void Verify()
		{
			const uint count = 10_000_000u;
			var array = Objects.Count.Default
			                   .Iteration()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Result()
			                   .Get(count)
			                   //.Get()
			                   ;
			array.Should().HaveCount(5);

			Objects.Count.Default.Get(count).Skip((int)(count - 5)).Take(5).Sum().Should().Be(array.Sum());

			/*var segment = Objects.Count.Default
			                          .Iterate()
			                          .Selection(x => x + 1)
			                          .Get(10_000);
			segment.Count.Should().Be(10_000);

			segment.ToArray().Sum(x => x).Should().Be(Objects.Count.Default.Get(10_000).Sum(x => x) + 10_000);*/
		}


	}
}