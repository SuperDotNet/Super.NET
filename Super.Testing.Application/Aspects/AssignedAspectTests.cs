using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Aspects;
using Super.Compose;
using Super.Model.Selection;
using System;
using Xunit;

namespace Super.Testing.Application.Aspects
{
	public class AssignedAspectTests
	{
		[Fact]
		void Verify()
		{
			AspectRegistry.Default.Execute(new Registration(typeof(AssignedAspect<,>)));

			var subject = Start.A.Selection<string>().By.Self;
			subject.Invoking(x => x.Get(null)).Should().NotThrow();
			subject.Configured()
			       .Invoking(x => x.Get(null))
			       .Should()
			       .Throw<InvalidOperationException>();
		}

		[Fact]
		void RuntimeRegistration()
		{
			var single    = new Registration(typeof(AssignedAspect<,>));
			var parameter = new[] {A.Type<string>(), A.Type<string>()};
			single.Get(parameter).Should().BeSameAs(AssignedAspect<string, string>.Default);
		}

		// TODO: Move to sequence testing.
		/*[Fact]
		void Count()
		{
			var first = 0;
			Enumerable.Range(0, 100)
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .FirstOrDefault();

			first.Should().Be(2);

			var second = 0;

			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .First()
			     .Get(Data.Default.Get());

			second.Should().Be(2);
		}*/

		public class Benchmarks
		{
			readonly ISelect<object, object> _subject;

			public Benchmarks() : this(A.Self<object>()) {}

			public Benchmarks(ISelect<object, object> subject) => _subject = subject;

			[Benchmark(Baseline = true)]
			public object Once() => _subject.Configured();
		}
	}
}