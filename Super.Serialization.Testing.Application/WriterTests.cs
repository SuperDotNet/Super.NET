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
			var expected = JsonSerializer.ToString(parameter);
			Encoding.UTF8.GetString(NumberWriter.Default.Get(parameter))
			        .Should()
			        .Be(expected);

			Encoding.UTF8.GetString(new Writer<uint>(PositiveNumber.Default, Leases<byte>.Default, 10).Get(parameter))
			        .Should()
			        .Be(expected);
		}

		/*sealed class Value
		{
			public Value(uint number) => Number = number;

			public uint Number { get; }
		}*/

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

			/*[Benchmark(Baseline = true)]
			public byte[] Native() => JsonSerializer.ToBytes(_data);*/

			[Benchmark]
			public byte[] Subject() => _writer.Get(_data);
		}
	}
}