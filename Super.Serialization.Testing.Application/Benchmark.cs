using BenchmarkDotNet.Attributes;
using Super.Model.Sequences;
using System.Text.Json.Serialization;

namespace Super.Serialization.Testing.Application
{
	public class Benchmark<T>
	{
		readonly IWriter<T> _subject;
		readonly T          _instance;

		public Benchmark(IInstruction instruction, T instance) : this(instruction.For<T>(), instance) {}

		public Benchmark(IInstruction<T> instruction, T instance)
			: this(new SingleInstructionWriter<T>(instruction), instance) {}

		public Benchmark(IWriter<T> subject, T instance)
		{
			_subject  = subject;
			_instance = instance;
		}

		[Benchmark(Baseline = true)]
		public Array<byte> Subject() => _subject.Get(_instance);

		[Benchmark]
		public Array<byte> Compare() => JsonSerializer.ToUtf8Bytes(_instance);
	}
}