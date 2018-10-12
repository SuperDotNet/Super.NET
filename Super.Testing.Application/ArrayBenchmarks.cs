using BenchmarkDotNet.Attributes;
using Super.Model.Collections;
using System.Collections.Immutable;

namespace Super.Testing.Application
{
	public class ArrayBenchmarks
	{
		readonly IArray<int> _native;
		readonly IArray<int> _chain;
		readonly IArray<int> _combo;
		/*readonly IArray<int> _expressionCombo;
		readonly IArray<int> _expression;*/

		public ArrayBenchmarks() : this(Objects.NativeArray.Default, Objects.Chain.Default, Objects.Combo.Default/*,
		                                Objects.ExpressionCombo.Default, Objects.Expression.Default*/) {}

		// ReSharper disable once TooManyDependencies
		public ArrayBenchmarks(IArray<int> native, IArray<int> chain, IArray<int> combo/*, IArray<int> expressionCombo,
		                       IArray<int> expression*/)
		{
			_native          = native;
			_chain           = chain;
			_combo           = combo;
			/*_expressionCombo = expressionCombo;
			_expression      = expression;*/
		}

		[Benchmark(Baseline = true)]
		public ImmutableArray<int> NativeArray() => _native.Get();

		[Benchmark]
		public ImmutableArray<int> Chain() => _chain.Get();

		[Benchmark]
		public ImmutableArray<int> Combo() => _combo.Get();

		/*[Benchmark]
		public ImmutableArray<int> ExpressionCombo() => _expressionCombo.Get().ToArray().ToImmutableArray();

		[Benchmark]
		public ImmutableArray<int> Expression() => _expression.Get().ToArray().ToImmutableArray();*/
	}
}