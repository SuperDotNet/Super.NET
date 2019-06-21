using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class StringValueInstructionTests
	{
		[Fact]
		void Verify()
		{
			const string data = "Hello World!";
			QuotedWriter.Default.Get(data.AsMemory()).Open().Should().Equal(JsonSerializer.ToUtf8Bytes(data));
		}

		sealed class QuotedWriter : SingleInstructionWriter<ReadOnlyMemory<char>>
		{
			public static QuotedWriter Default { get; } = new QuotedWriter();

			QuotedWriter() : base(StringInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : Benchmark<ReadOnlyMemory<char>>
		{
			public Benchmarks() : base(QuotedWriter.Default, "Hello World!".AsMemory()) {}
		}
	}
}