using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
{
	public sealed class SingleInstructionWriterTests
	{
		[Fact]
		void Verify()
		{
			const uint number = 12345;
			Writer.Default.Get(number)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(number));
		}

		sealed class Writer : SingleInstructionWriter<uint>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(PositiveIntegerInstruction.Default) {}
		}

		public class Benchmarks
		{
			readonly IWriter<uint> _subject;
			readonly IWriter<uint> _compare;
			readonly uint          _data;

			public Benchmarks() : this(Writer.Default, new Writer<uint>(PositiveInteger.Default, 10), 12345) {}

			public Benchmarks(IWriter<uint> subject, IWriter<uint> compare, uint data)
			{
				_subject = subject;
				_compare = compare;
				_data    = data;
			}

			[Benchmark(Baseline = true)]
			public Array<byte> Compare() => _compare.Get(_data);

			[Benchmark]
			public Array<byte> Subject() => _subject.Get(_data);
		}
	}
}