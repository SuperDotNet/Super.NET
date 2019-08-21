using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Text.Json;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class EscapingStringInstructionTests
	{
		[Fact]
		void Verify()
		{
			const string data = "Hello World!";
			QuotedWriter.Default.Get(data).Open().Should().Equal(JsonSerializer.SerializeToUtf8Bytes(data));
		}

		/*[Fact]
		void VerifyUnicode()
		{
			var data = $"{(char)0xD800}{(char)0xDC00}";
			QuotedWriter.Default.Get(data).Open().Should().Equal(JsonSerializer.SerializeToUtf8Bytes(data));
		}*/

		sealed class QuotedWriter : SingleInstructionWriter<string>
		{
			public static QuotedWriter Default { get; } = new QuotedWriter();

			QuotedWriter() : base(new StringInstruction(VerboseTextEncoder.Default).Quoted()) {}
		}

		public class Benchmarks : ComparisonBenchmark<string>
		{
			public Benchmarks() : base(QuotedWriter.Default, $"Hello Unicode: {(char)0xD800}{(char)0xDC00}") {}
		}
	}
}