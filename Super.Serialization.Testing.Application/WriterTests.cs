using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using System.Text;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public class WriterTests
	{
		[Fact]
		public void Simple()
		{
			const uint parameter = 12345u;
			Encoding.UTF8.GetString(Writer.Default.Get(parameter))
			        .Should()
			        .Be(JsonSerializer.ToString(parameter));
		}

		sealed class Writer : Writer<uint>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(IntegerInstruction.Default) {}
		}

		public class Benchmarks
		{
			readonly IWriter<uint> _writer;
			readonly uint          _data;

			public Benchmarks() : this(Writer.Default, 12345u) {}

			public Benchmarks(IWriter<uint> writer, uint data)
			{
				_writer = writer;
				_data   = data;
			}

			[Benchmark]
			public byte[] Native() => JsonSerializer.ToUtf8Bytes(_data);

			[Benchmark(Baseline = true)]
			public Array<byte> Array() => _writer.Get(_data);
		}
	}
}