using FluentAssertions;
using System.Buffers;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class PositiveNumberTests
	{
		[Fact]
		void Verify()
		{
			PositiveNumber.Default.Get(new Composition<uint>(ArrayPool<byte>.Shared.Rent(5), 1234567))
			              .Length.Should()
			              .Be(7);
		}

		[Fact]
		void VerifyAllocated()
		{
			PositiveNumber.Default.Get(new Composition<uint>(new byte[30], 1234567))
			              .Length.Should()
			              .Be(7);
		}
	}
}