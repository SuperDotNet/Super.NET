using FluentAssertions;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
using System.Linq;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class ArrayInstructionTests
	{
		[Fact]
		void Verify()
		{
			var instances = new[] {"Hello", "World!"};

			Writer.Default.Get(instances).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(instances));
		}

		sealed class Writer : SingleInstructionWriter<Array<string>>, IWriter<string[]>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(new ArrayInstruction<string>(StringInstruction.Default.Quoted())) {}

			public Array<byte> Get(string[] parameter) => base.Get(parameter);
		}

		public class Benchmarks : ComparisonBenchmark<string[]>
		{
			public Benchmarks() : base(Writer.Default, Enumerable.Repeat("Hello World!", 100).ToArray()) {}
		}
	}
}