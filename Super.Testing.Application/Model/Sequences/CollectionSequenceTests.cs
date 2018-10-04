using FluentAssertions;
using Super.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class CollectionSequenceTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			In<IList<int>>.Start()
			              .Sequence()
			              .Get()
			              .Get(expected.ToList())
			              .Should()
			              .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			In<IList<int>>.Start()
			              .Sequence()
			              .Skip(5000)
			              .Take(300)
			              .Get()
			              .Get(source.ToList())
			              .Should()
			              .Equal(expected);
		}
	}
}