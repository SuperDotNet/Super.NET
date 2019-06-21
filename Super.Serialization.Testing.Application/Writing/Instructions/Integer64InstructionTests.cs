using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class Integer64InstructionTests
	{
		[Fact]
		void Verify()
		{
			const ulong number = 12345678910111213;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		[Fact]
		void VerifyNegative()
		{
			const long number = -1211109876543210;
			new SingleInstructionWriter<long>(FullInteger64Instruction.Default)
				.Get(number)
				.Open()
				.Should()
				.Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<ulong>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(Integer64Instruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<ulong>
		{
			public Benchmarks() : base(Writer.Default, 12345678910111213) {}
		}
	}
}