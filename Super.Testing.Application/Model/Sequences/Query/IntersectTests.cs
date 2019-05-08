using FluentAssertions;
using Super.Compose;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query
{
	public sealed class IntersectTests
	{
		[Fact]
		void Verify()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Intersect(Sequence.From(second))
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Intersect(second));
		}
	}
}