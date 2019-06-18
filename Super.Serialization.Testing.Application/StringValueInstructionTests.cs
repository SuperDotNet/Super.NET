using FluentAssertions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
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

			QuotedWriter() : base(StringValueInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : Benchmark<string>
		{
			public Benchmarks() : base(StringValueInstruction.Default, "Hello World!") {}
		}
	}
}