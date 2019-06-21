using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class ShortInstructionTests
	{
		[Fact]
		void Verify()
		{
			const ushort number = 12345;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		[Fact]
		void VerifyNegative()
		{
			const short number = -12345;
			new SingleInstructionWriter<short>(FullShortInstruction.Default).Get(number)
			                                                                .Open()
			                                                                .Should()
			                                                                .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<ushort>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(ShortInstruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<ushort>
		{
			public Benchmarks() : base(Writer.Default, 123) {}
		}
	}
}