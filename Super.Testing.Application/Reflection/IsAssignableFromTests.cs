using FluentAssertions;
using Super.Reflection.Types;
using System.IO;
using Xunit;
using Stream = System.IO.Stream;

namespace Super.Testing.Application.Reflection
{
	public class IsAssignableFromTests
	{
		[Fact]
		public void Verify()
		{
			IsAssignableFrom<Stream>.Default
			                        .Get(typeof(MemoryStream))
			                        .Should()
			                        .BeTrue();
		}
	}
}