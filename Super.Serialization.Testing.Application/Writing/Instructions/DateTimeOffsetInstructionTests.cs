using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
    public sealed class DateTimeOffsetInstructionTests
	{
		[Fact]
		void Verify()
		{
			var time = new DateTimeOffset(new DateTime(1976, 6, 7, 23, 17, 36),
			                              TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset);
			Writer.Default.Get(time)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(time));
		}

		sealed class Writer : SingleInstructionWriter<DateTimeOffset>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(DateTimeOffsetInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : ComparisonBenchmark<DateTimeOffset>
		{
			public Benchmarks() : base(DateTimeOffsetInstruction.Default.Quoted(),
			                           new DateTimeOffset(new DateTime(1976, 6, 7, 23, 17, 36),
			                                              TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
			                                                          .BaseUtcOffset)) {}
		}
	}
}