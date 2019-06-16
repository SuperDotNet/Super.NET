using FluentAssertions;
using System.Buffers;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class PositiveIntegerTests
	{
		[Fact]
		void Verify()
		{
			PositiveInteger.Default.Get(new Composition<uint>(ArrayPool<byte>.Shared.Rent(5), 1234567))
			              .Index.Should()
			              .Be(7);
		}

		[Fact]
		void VerifyAllocated()
		{
			PositiveInteger.Default.Get(new Composition<uint>(new byte[30], 1234567))
			              .Index.Should()
			              .Be(7);
		}
	}
}