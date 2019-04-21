using BenchmarkDotNet.Attributes;
using Super.Model.Commands;
using Super.Model.Sequences;
using Super.Runtime;
// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class SessionTests
	{
		public class Benchmarks
		{
			readonly Session<object>
				_null = new Session<object>(new Store<object>(Empty<object>.Array), null),
			    _empty = new Session<object>(new Store<object>(Empty<object>.Array), EmptyCommand<object[]>.Default);

			[Benchmark]
			public void Empty() => _empty.Dispose();

			[Benchmark]
			public void Null() => _null.Dispose();
		}
	}
}