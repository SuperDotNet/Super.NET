using BenchmarkDotNet.Attributes;

namespace Super.Testing.Application.Model
{
	public class AssignedTests
	{
		// ReSharper disable ImpureMethodCallOnReadonlyValueField
		public class Benchmarks
		{
			readonly Super.Model.Assigned<uint> _sut = new Super.Model.Assigned<uint>(6776u);

			readonly int _other;

			public Benchmarks() : this(6776) {}

			public Benchmarks(int other) => _other = other;

			[Benchmark]
			public uint Or() => _sut.Or((uint)_other);

			[Benchmark]
			public uint OrReference()
			{
				var other = (uint)_other;
				return _sut.Or(in other);
			}

			[Benchmark]
			public uint OrLocal()
			{
				var assigned = _sut;
				return assigned.Or((uint)_other);
			}

			[Benchmark]
			public uint OrLocalReference()
			{
				var assigned = _sut;
				var other = (uint)_other;
				return assigned.Or(in other);
			}

			[Benchmark(Baseline = true)]
			public uint Ternary() => _sut.IsAssigned ? _sut.Instance : (uint)_other;
		}
	}
}