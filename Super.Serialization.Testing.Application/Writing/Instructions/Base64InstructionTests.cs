using FluentAssertions;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public class Base64InstructionTests
	{
		[Theory]
		[InlineData("Hello World!")]
		[InlineData("Hello\nWorld!")]
		void Verify(string element)
		{
			var data = Encoder.Default.Get(element);
			Writer.Default.Get(data).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(data));
		}

		sealed class Writer : SingleInstructionWriter<Array<byte>>, IWriter<byte[]>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(Base64Instruction.Default.Quoted()) {}

			public Array<byte> Get(byte[] parameter) => base.Get(parameter);
		}

		public class Benchmarks : ComparisonBenchmark<byte[]>
		{
			public Benchmarks() : base(Writer.Default, Encoder.Default.Get("Hello World!")) {}
		}
	}
}