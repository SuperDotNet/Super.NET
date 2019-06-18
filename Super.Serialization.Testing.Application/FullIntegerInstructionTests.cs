using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
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

		public class Benchmarks : Benchmark<int>
		{
			public Benchmarks() : base(Writer.Default, -123456) {}
		}
	}
}