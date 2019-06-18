using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
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

		public class Benchmarks : Benchmark<float>
		{
			public Benchmarks() : base(FloatInstruction.Default, 12.34567f) {}
		}
	}
}