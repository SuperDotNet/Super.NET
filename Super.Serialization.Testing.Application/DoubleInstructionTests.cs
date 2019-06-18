using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public sealed class DoubleInstructionTests
	{
		[Fact]
		void Verify()
		{
			const double value = -12.34567f;
			Writer.Default.Get(value)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(value));
		}

		sealed class Writer : SingleInstructionWriter<double>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(DoubleInstruction.Default) {}
		}

		public class Benchmarks : Benchmark<double>
		{
			public Benchmarks() : base(DoubleInstruction.Default, -12.34567f) {}
		}
	}
}