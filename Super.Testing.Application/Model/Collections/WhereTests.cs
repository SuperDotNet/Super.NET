// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{

		[Fact]
		void Verify()
		{
			const uint count = 10_000_000u;
			var array = Objects.Count.Default
			                   .Iterate()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Get(count)
			                   .Get()
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