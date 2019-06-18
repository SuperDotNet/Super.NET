using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
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
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		[Fact]
		void VerifyNegative()
		{
			const sbyte number = -123;
			new SingleInstructionWriter<sbyte>(FullByteInstruction.Default).Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<byte>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(ByteInstruction.Default) {}
		}

		public class Benchmarks : Benchmark<byte>
		{
			public Benchmarks() : base(Writer.Default, 123) {}
		}
	}
}