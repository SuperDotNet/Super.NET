using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class ByteInstructionTests
	{
		[Fact]
		void Verify()
		{
			const byte number = 123;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.SerializeToUtf8Bytes(number));
		}

		[Fact]
		void VerifyNegative()
		{
			const sbyte number = -123;
			new SingleInstructionWriter<sbyte>(FullByteInstruction.Default).Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.SerializeToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<byte>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(ByteInstruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<byte>
		{
			public Benchmarks() : base(Writer.Default, 123) {}
		}
	}
}