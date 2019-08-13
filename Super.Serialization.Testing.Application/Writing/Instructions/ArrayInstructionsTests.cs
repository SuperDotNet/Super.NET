using FluentAssertions;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class ArrayInstructionsTests
	{
		[Fact]
		void Verify()
		{
			var instances = new[] {"Hello", "World!"};

			Writer.Default.Get(instances).Open().Should().Equal(JsonSerializer.SerializeToUtf8Bytes(instances));
		}

		sealed class Writer : RuntimeWriter<Array<string>>, IWriter<string[]>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : this(StringInstruction.Default.Quoted()) {}

			public Writer(IInstruction<string> element)
				: this(Enumerable.Range(0, 100)
				                 .Select(x => new ArrayElementInstruction<string>(element, (uint)x))
				                 .ToArray<IInstruction<Array<string>>>()) {}

			public Writer(params IInstruction<Array<string>>[] instructions)
				: this(new ElementInstructions<string>(instructions)) {}

			public Writer(IElementInstructions<string> instructions)
				: base(new ArrayInstructions<string>(instructions)) {}

			public Array<byte> Get(string[] parameter) => base.Get(parameter);
		}

		public class Benchmarks : ComparisonBenchmark<string[]>
		{
			public Benchmarks() : base(Writer.Default, Enumerable.Repeat("Hello World!", 100).ToArray()) {}
		}
	}
}