using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class HasTests
	{
		[Fact]
		public void Verify()
		{
			var item = new object();
			var items = item.Yield()
			                .ToList();
			new Has<object>(items).IsSatisfiedBy(item)
			                                        .Should()
			                                        .BeTrue();
		}
	}
}