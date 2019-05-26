using System.IO;
using FluentAssertions;
using Super.Reflection.Types;
using Xunit;

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