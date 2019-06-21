using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class DateTimeInstructionTests
	{
		[Fact]
		void Verify()
		{
			var time = new DateTime(1976, 6, 7, 23, 17, 36);
			Writer.Default.Get(time)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(time));
		}

		sealed class Writer : SingleInstructionWriter<DateTime>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(DateTimeInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : ComparisonBenchmark<DateTime>
		{
			public Benchmarks() : base(DateTimeInstruction.Default.Quoted(), new DateTime(1976, 6, 7, 23, 17, 36)) {}
		}
	}
}