using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class FloatInstructionTests
	{
		[Fact]
		void Verify()
		{
			const float value = 12.34567f;
			Writer.Default.Get(value)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(value));
		}

		sealed class Writer : SingleInstructionWriter<float>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(FloatInstruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<float>
		{
			public Benchmarks() : base(FloatInstruction.Default, 12.34567f) {}
		}
	}
}