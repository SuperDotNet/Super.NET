using FluentAssertions;
using Super.Model.Specifications;
using Super.Reflection.Types;
using System.IO;
using Xunit;

namespace Super.Testing.Reflection
{
	public class IsAssignableFromTests
	{
		[Fact]
		public void Verify()
		{
			IsAssignableFrom<Stream>.Default.IsSatisfiedBy(typeof(MemoryStream))
			                                 .Should()
			                                 .BeTrue();
		}
	}
}