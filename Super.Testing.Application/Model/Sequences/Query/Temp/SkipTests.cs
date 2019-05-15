using FluentAssertions;
using Super.Model.Sequences.Query.Temp;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query.Temp
{
	public sealed class SkipTests
	{
		[Fact]
		void Verify()
		{
			var elements = new[]
			{
				0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8,
				8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9
			};

			new Build.Skip<int>(10).Get()
			                        .Get(elements)
			                        .ToArray()
			                        .Should()
			                        .Equal(elements.Skip(10));
		}
	}
}