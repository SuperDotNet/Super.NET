using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
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

		public class Benchmarks
		{
			readonly IWriter<string> _quoted;
			readonly string          _data;

			public Benchmarks() : this(QuotedWriter.Default, "Hello World!") {}

			public Benchmarks(IWriter<string> quoted, string data)
			{
				_quoted    = quoted;
				_data      = data;
			}

			[Benchmark]
			public byte[] Native() => JsonSerializer.ToUtf8Bytes(_data);

			[Benchmark(Baseline = true)]
			public Array<byte> Quoted() => _quoted.Get(_data);
		}
	}
}