// ReSharper disable ComplexConditionExpression

using Super.Model.Collections;
using Super.Model.Selection;
using System.Reactive;
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
			Objects.Data.Default.Get().ToSource()
			       .Out()
			       .AsSelect()
			       .Iterate().Where(x => x.Length > 1000)
			       .Get(Unit.Default)
			       .Allocate();
		}


	}
}