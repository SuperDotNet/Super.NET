using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public class FullIntegerInstructionTests
	{
		[Fact]
		void Verify()
		{
			const int number = 12345;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		[Fact]
		void VerifyNegative()
		{
			const int number = -12345;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<int>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(FullIntegerInstruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<int>
		{
			public Benchmarks() : base(Writer.Default, -123456) {}
		}
	}
}