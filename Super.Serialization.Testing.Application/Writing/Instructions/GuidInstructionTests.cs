using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System;
using System.Text.Json;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class GuidInstructionTests
	{
		[Fact]
		void Verify()
		{
			var value = Guid.NewGuid();
			Writer.Default.Get(value)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.SerializeToUtf8Bytes(value));
		}

		sealed class Writer : SingleInstructionWriter<Guid>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(GuidInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : ComparisonBenchmark<Guid>
		{
			public Benchmarks() : base(GuidInstruction.Default.Quoted(), Guid.NewGuid()) {}
		}
	}
}