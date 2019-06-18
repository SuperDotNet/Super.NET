using FluentAssertions;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class PositiveIntegerTests
	{
		/*[Fact]
		void Verify()
		{
			PositiveIntegerInstruction.Default
			                          .Get(new Composition<uint>(ArrayPool<byte>.Shared.Rent(5), 1234567))
			                          .Should()
			                          .Be(7);
		}*/

		[Fact]
		void VerifyAllocated()
		{
			PositiveIntegerInstruction.Default
			                          .Get(new Composition<uint>(new byte[30], 1234567))
			                          .Should()
			                          .Be(7);
		}
	}
}