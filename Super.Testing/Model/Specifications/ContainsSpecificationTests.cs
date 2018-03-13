using System.Linq;
using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Xunit;

namespace Super.Testing.Model.Specifications
{
	public class ContainsSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			var item = new object();
			var items = item.Yield()
			                .ToList();
			new ContainsSpecification<object>(items).IsSatisfiedBy(item)
			                                        .Should()
			                                        .BeTrue();
		}
	}
}