using BenchmarkDotNet.Attributes;
using Super.Model.Collections;
using System.Collections.Immutable;

namespace Super.Testing.Application
{
	public class ArrayBenchmarks
	{
		readonly IArrays<int> _native;
		readonly IArrays<int> _chain;
		readonly IArrays<int> _combo;
		/*readonly IArray<int> _expressionCombo;
		readonly IArray<int> _expression;*/

		public ArrayBenchmarks() : this(Objects.NativeArrays.Default, Objects.Chain.Default, Objects.Combo.Default/*,
		                                Objects.ExpressionCombo.Default, Objects.Expression.Default*/) {}

		// ReSharper disable once TooManyDependencies
		public ArrayBenchmarks(IArrays<int> native, IArrays<int> chain, IArrays<int> combo/*, IArray<int> expressionCombo,
		                       IArray<int> expression*/)
		{
			_native          = native;
			_chain           = chain;
			_combo           = combo;
			/*_expressionCombo = expressionCombo;
			_expression      = expression;*/
		}

		[Benchmark(Baseline = true)]
		public ImmutableArray<int> NativeArray() => _native.Get().ToArray().ToImmutableArray();

		[Benchmark]
		public ImmutableArray<int> Chain() => _chain.Get().ToArray().ToImmutableArray();

		[Benchmark]
		public ImmutableArray<int> Combo() => _combo.Get().ToArray().ToImmutableArray();

		/*[Benchmark]
		public ImmutableArray<int> ExpressionCombo() => _expressionCombo.Get().ToArray().ToImmutableArray();

		[Benchmark]
		public ImmutableArray<int> Expression() => _expression.Get().ToArray().ToImmutableArray();*/
	}
}