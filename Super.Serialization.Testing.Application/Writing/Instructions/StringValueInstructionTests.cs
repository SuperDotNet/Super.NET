using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class StringValueInstructionTests
	{
		[Fact]
		void Verify()
		{
			const string data = "Hello World!";
			QuotedWriter.Default.Get(data).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(data));
		}

		sealed class QuotedWriter : SingleInstructionWriter<string>
		{
			public static QuotedWriter Default { get; } = new QuotedWriter();

			QuotedWriter() : base(StringInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : Benchmark<string>
		{
			public Benchmarks() : base(QuotedWriter.Default, "Hello World!") {}
		}
	}
}