using FluentAssertions;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public class CloneTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			new Clone<int>(Allocated<int>.Default).Get(expected).Should().Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			new Clone<int>(Allocated<int>.Default, new Super.Model.Collections.Selection(1000, 250))
				.Get(source)
				.Should()
				.Equal(expected);
		}
	}
}