using FluentAssertions;
using System;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application
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
			      .Equal(JsonSerializer.ToUtf8Bytes(value));
		}

		sealed class Writer : SingleInstructionWriter<Guid>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(GuidInstruction.Default.Quoted()) {}
		}

		public class Benchmarks : Benchmark<Guid>
		{
			public Benchmarks() : base(GuidInstruction.Default.Quoted(), Guid.NewGuid()) {}
		}
	}
}