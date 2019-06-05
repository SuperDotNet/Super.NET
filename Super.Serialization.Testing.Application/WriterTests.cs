using BenchmarkDotNet.Attributes;
using FluentAssertions;
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
			Encoding.UTF8.GetString(NumberWriter.Default.Get(12345u))
			        .Should()
			        .Be(JsonSerializer.ToString(12345u));
		}

		sealed class Value
		{
			public Value(uint number) => Number = number;

			public uint Number { get; }
		}

		public class Benchmarks
		{
			readonly IWriter<uint> _writer;
			readonly uint          _data;

			public Benchmarks() : this(NumberWriter.Default, 12345) {}

			public Benchmarks(IWriter<uint> writer, uint data)
			{
				_writer = writer;
				_data   = data;
			}

			[Benchmark(Baseline = true)]
			public byte[] Native() => JsonSerializer.ToBytes(_data);

			[Benchmark]
			public byte[] Subject() => _writer.Get(_data);
		}
	}
}