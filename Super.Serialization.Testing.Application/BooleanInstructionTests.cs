using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class BooleanInstructionTests
	{
		[Fact]
		void Verify()
		{
			Writer.Default.Get(true)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(true));
		}

		[Fact]
		void VerifyFalse()
		{
			Writer.Default.Get(false)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(false));
		}

		sealed class Writer : SingleInstructionWriter<bool>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(BooleanInstruction.Default) {}
		}

		public class Benchmarks : Benchmark<bool>
		{
			public Benchmarks() : this(true) {}

			public Benchmarks(bool instance)
				: this(instance ? (IInstruction)TrueInstruction.Default : FalseInstruction.Default, instance) {}

			public Benchmarks(IInstruction subject, bool instance) : base(subject, instance) {}
		}
	}
}