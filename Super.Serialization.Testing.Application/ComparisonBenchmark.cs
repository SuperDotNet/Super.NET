using BenchmarkDotNet.Attributes;
using Super.Model.Sequences;
using Super.Serialization.Writing.Instructions;
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
	}

	public class ComparisonBenchmark<T> : Benchmark<T>
	{
		readonly T _instance;

		public ComparisonBenchmark(IInstruction instruction, T instance) : this(instruction.For<T>(), instance) {}

		public ComparisonBenchmark(IInstruction<T> instruction, T instance)
			: this(new SingleInstructionWriter<T>(instruction), instance) {}

		public ComparisonBenchmark(IWriter<T> subject, T instance) : base(subject, instance) => _instance = instance;

		[Benchmark]
		public Array<byte> Compare() => JsonSerializer.ToUtf8Bytes(_instance);
	}
}