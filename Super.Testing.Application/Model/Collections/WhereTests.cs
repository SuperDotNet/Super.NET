// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{
		/*readonly static string[] Strings = Objects.Data.Default.Get();

		readonly ISelect<Unit, View<int>> _load = Strings.ToSource()
		                                                 .Out()
		                                                 .AsSelect()
		                                                 .Iterate()
		                                                 .Selection(x => x.Length);*/
		[Fact]
		void Verify()
		{
			/*var expected = Enumerable.Range(0, 10_000).Select(x => x).ToArray();

			var buffer  = new Buffer<int>(10);
			var current = buffer;
			for (var i = 0; i < expected.Length; i++)
			{
				current.Append(i);
			}
			current.Flush().Should().Equal(expected);*/
			/*Objects.Data.Default.Get().ToSource()
			       .Out()
			       .AsSelect()
			       .Iterate().Where(x => x.Length > 1000)
			       .Get(Unit.Default)
			       .Allocate();*/
			/*Objects.Count.Default
			       .Iterate()
			       /*.Skip(10_000_000 - 5)
			       .Take(5)#1#
			       .Get(10_000_000).Allocate().Should().HaveCount(5);*/
			var segment = Objects.Count.Default
			                          .Iterate2()
			                          .Selection2(x => x + 1)
			                          .Get(10_000);
			segment.Count.Should().Be(10_000);

			segment.ToArray().Sum(x => x).Should().Be(Objects.Count.Default.Get(10_000).Sum(x => x) + 10_000);
		}


	}
}