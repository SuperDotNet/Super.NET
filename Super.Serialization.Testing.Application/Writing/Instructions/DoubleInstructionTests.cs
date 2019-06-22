using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
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

		public class Benchmarks : ComparisonBenchmark<double>
		{
			public Benchmarks() : base(DoubleInstruction.Default, -12.34567f) {}
		}
	}
}