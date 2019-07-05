using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Serialization.Writing.Instructions;
using System.Linq;
using System.Text.Json.Serialization;
using Xunit;

namespace Super.Serialization.Testing.Application.Writing.Instructions
{
	public sealed class DefaultStringInstructionTests
	{
		[Fact]
		void Verify()
		{
			var content = string.Join('\n', Enumerable.Repeat("Hello World!", 3));
			Writer.Default.Get(content)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(content));
		}

		[Fact]
		void VerifyNormal()
		{
			var content = string.Join('-', Enumerable.Repeat("Hello World!", 10));
			Writer.Default.Get(content)
			      .Open()
			      .Should()
			      .Equal(JsonSerializer.ToUtf8Bytes(content));
		}

		sealed class Writer : SingleInstructionWriter<string>
		{
			public static Writer Default { get; } = new Writer();

			Writer() : base(new StringInstruction(VerboseTextEncoder.Default).Quoted()) {}
		}

		public class Benchmarks : Benchmark<string>
		{
			readonly string _instance;

			public Benchmarks() : this(string.Join('\n', Enumerable.Repeat("Hello World!", 2))) {}

			public Benchmarks(string instance) : base(Writer.Default, instance) => _instance = instance;

			[Benchmark]
			public byte[] Native() => JsonSerializer.ToUtf8Bytes(_instance);
		}
	}
}