using BenchmarkDotNet.Attributes;
using FluentAssertions;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
			Encoding.UTF8.GetString(Writer.Default.Get(parameter))
			        .Should()
			        .Be(expected);

			Encoding.UTF8.GetString(new Writer<uint>(PositiveNumber.Default, 10).Get(parameter))
			        .Should()
			        .Be(expected);
		}

		[Fact]
		public async Task Measure()
		{
			const uint parameter = 12345u;
			using (var stream = new MemoryStream())
			{
				await JsonSerializer.WriteAsync(parameter, stream);
				stream.ToArray().Should().Equal(JsonSerializer.ToUtf8Bytes(parameter));
			}
		}

		/*sealed class Value
		{
			public Value(uint number) => Number = number;

			public uint Number { get; }
		}*/

		sealed class Writer : Writer<uint>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(PositiveNumber.Default) {}
		}

		public class Benchmarks
		{
			readonly IWriter<uint> _writer;
			readonly uint          _data;

			public Benchmarks() : this(Writer.Default, 12345) {}

			public Benchmarks(IWriter<uint> writer, uint data)
			{
				_writer = writer;
				_data   = data;
			}

			[Benchmark]
			public byte[] Subject() => _writer.Get(_data);

			[Benchmark(Baseline = true)]
			public byte[] Native() => JsonSerializer.ToUtf8Bytes(_data);
		}
	}
}