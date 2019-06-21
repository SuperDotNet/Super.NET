using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
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
			IntegerInstruction.Default
			                          .Get(new Composition<uint>(new byte[30], 1234567))
			                          .Should()
			                          .Be(7);
		}
	}
}