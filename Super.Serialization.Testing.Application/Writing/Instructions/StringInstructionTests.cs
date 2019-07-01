using FluentAssertions;
using Newtonsoft.Json;
using Super.Serialization.Writing.Instructions;
using System.Linq;
using Xunit;
using JsonSerializer = System.Text.Json.Serialization.JsonSerializer;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public class StringInstructionTests
	{
		[Fact]
		void Verify()
		{
			string data = string.Join('-', Enumerable.Repeat("Hello World!", 1000));
			Writer.Default.Get(data).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(data));
		}

		[Fact]
		void VerifyEscape()
		{
			const string data = "Hello World!\nHello World Again!";
			Writer.Default.Get(data).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(data));
		}

		[Fact]
		void VerifyUnicode()
		{
			const string data      = "Hello World!\nHello World Again: 方!";
			var          parameter = Writer.Default.Get(data).Open();
			Encoder.Default.Get(parameter)
			       .Should()
			       .Be(JsonConvert.ToString(data));
			parameter.Should().NotEqual(JsonSerializer.ToUtf8Bytes(data));
		}

		[Fact]
		void VerifyDoubleUnicode()
		{
			var data      = $"Hello Unicode: {(char)0xD800}{(char)0xDC00}";
			var parameter = Writer.Default.Get(data).Open();
			Encoder.Default.Get(parameter)
			       .Should()
			       .Be(JsonConvert.ToString(data));
			parameter.Should().NotEqual(JsonSerializer.ToUtf8Bytes(data));
		}

		sealed class Writer : SingleInstructionWriter<string>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(StringInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : ComparisonBenchmark<string>
		{
			public Benchmarks() : base(Writer.Default, string.Join('\n', Enumerable.Repeat("Hello World!", 1000))) {}
		}
	}
}