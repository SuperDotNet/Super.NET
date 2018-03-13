using System.IO;
using FluentAssertions;
using Super.Model.Specifications;
using Super.Reflection;
using Xunit;

namespace Super.Testing.Reflection
{
	public class IsAssignableSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			IsAssignableSpecification<Stream>.Default.IsSatisfiedBy(typeof(MemoryStream))
			                                 .Should()
			                                 .BeTrue();
		}
	}
}