using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class DecimalInstructionTests
	{
		[Fact]
		void Verify()
		{
			const decimal value = -12.34567m;
			Writer.Default.Get(value)
				  .Open()
				  .Should()
				  .Equal(JsonSerializer.ToUtf8Bytes(value));
		}

		sealed class Writer : SingleInstructionWriter<decimal>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(DecimalInstruction.Default) {}
		}

		public class Benchmarks : ComparisonBenchmark<decimal>
		{
			public Benchmarks() : base(DecimalInstruction.Default, -12.34567m) {}
		}
	}
}