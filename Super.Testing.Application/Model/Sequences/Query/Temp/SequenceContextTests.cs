﻿using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Humanizer;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Sequences.Query;
using Super.Model.Sequences.Query.Construction;
using System.Linq;
using Xunit;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Super.Testing.Application.Model.Sequences.Query.Temp
{
	public sealed class SequenceContextTests
	{
		const int skip = 10, take = 7;

		readonly static int[] data =
		{
			0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8,
			8, 9, 9, 9, 9, 9, 9, 9, 9, 9
		};

		[Fact]
		void Verify()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                      .Get(new Take(take))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Take(take))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();
		}

		[Fact]
		void VerifySkipWhere()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                      .Get(new Build.Where<int>(x => x == 5))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Where(x => x == 5))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();
		}

		[Fact]
		void VerifySkipTakeWhere()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                      .Get(new Take(take))
			                                      .Get(new Build.Where<int>(x => x == 5))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Take(take).Where(x => x == 5))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();
		}

		[Fact]
		void VerifySkipWhereTake()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                      .Get(new Build.Where<int>(x => x == 5))
			                                      .Get(new Take(take))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Where(x => x == 5).Take(take))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();
		}

		[Fact]
		void VerifyWhere()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 8))
			                                      .Get()
			                                      .Get(data)
			                                      .Should()
			                                      .NotBeEmpty()
			                                      .And.Subject.Should()
			                                      .Equal(data.Where(x => x > 8));
		}

		[Fact]
		void VerifySelect()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                      .Get()
			                                      .Get(data)
			                                      .Should()
			                                      .NotBeEmpty()
			                                      .And.Subject.Should()
			                                      .Equal(data.Select(x => x.ToString()));
		}

		[Fact]
		void VerifyComparison()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Where(x => x > 8)
			     .Select(x => x.ToString())
			     .Get(data)
			     .Open()
			     .Should()
			     .Equal(data.Where(x => x > 8).Select(x => x.ToString()));
		}

		[Fact]
		void VerifyActivation()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.StoredActivation<Super.Model.Sequences.Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Instantiation<Super.Model.Sequences.Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);
		}

		[Fact]
		void VerifyWhereSelect()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 8))
			                                      .Get(new Build.Select<int, string>(x => x.ToString()))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Where(x => x > 8).Select(x => x.ToString()));
		}

		[Fact]
		void VerifySelectSelect()
		{
			new Start<int[], int>(A.Self<int[]>())
				.Get(new Build.Select<int, int>(x => x + 1))
				.Get(new Build.Select<int, string>(x => x.ToString()))
				.Get(data)
				.Should()
				.Equal(data.Select(x => x + 1).Select(x => x.ToString()));
		}

		[Fact]
		void VerifySelectSelectFirst()
		{
			var element = new Start<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x => x + 1))
			                                                    .Get(new Build.Select<int, string>(x => x.ToWords()))
			                                                    .Get(FirstOrDefault<string>.Default)
			                                                    .Get(data);
			element.Should().Be(data.Select(x => x + 1).Select(x => x.ToWords()).FirstOrDefault());
		}

		[Fact]
		void VerifyCountBasic()
		{
			{
				var count = 0;
				Enumerable.Range(0, 100)
				          .Select(x =>
				                  {
					                  count++;
					                  return x;
				                  })
				          .Select(x =>
				                  {
					                  count++;
					                  return x;
				                  })
				          .FirstOrDefault();

				count.Should().Be(2);
			}

			{
				var count = 0;

				new Start<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x =>
				                                                                      {
					                                                                      count++;
					                                                                      return x;
				                                                                      }))
				                                      .Get(new Build.Select<int, int>(x =>
				                                                                      {
					                                                                      count++;
					                                                                      return x;
				                                                                      }))
				                                      .Get(FirstOrDefault<int>.Default)
				                                      .Get(data);
				count.Should().Be(2);
			}
		}

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

			second.Should().Be(4);
		}*/

		public class Benchmarks
		{
			readonly ISelect<int[], string> _subject;
			// ReSharper disable once NotAccessedField.Local
			readonly ISelect<int[], string> _current;

			public Benchmarks() : this(new Start<int[], int>(A.Self<int[]>())
			                           //.Get(new Build.Where<int>(x => x > 8))
			                           .Get(new Build.Select<int, string>(x => x.ToString()))
			                           .Get(FirstOrDefault<string>.Default)
			                           ,
			                           Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Array.By.Self.Query()
			                             //   .Where(x => x > 8)
			                                .Select(x => x.ToString())
			                                .First()) {}

			public Benchmarks(ISelect<int[], string> subject, ISelect<int[], string> current)
			{
				_subject = subject;
				_current = current;
			}

			[Benchmark(Baseline = true)]
			public string Classic() => data/*.Where(x => x > 8)*/.Select(x => x.ToString()).FirstOrDefault();

			/*[Benchmark]
			public string Current() => _current.Get(data);*/

			[Benchmark]
			public string Proposed() => _subject.Get(data);
		}

		/*public class Benchmarks
		{
			readonly ISelect<int[], string[]> _subject;
			readonly ISelect<int[], string[]> _current;

			public Benchmarks() : this(new Start<int[], int>(A.Self<int[]>())
			                           .Get(new Build.Where<int>(x => x > 8))
			                           .Get(new Build.Select<int, string>(x => x.ToString()))
			                           .Get(),
			                           Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Array.By.Self.Query()
			                                .Where(x => x > 8)
			                                .Select(x => x.ToString())
			                                .Out()) {}

			public Benchmarks(ISelect<int[], string[]> subject, ISelect<int[], string[]> current)
			{
				_subject = subject;
				_current = current;
			}

			[Benchmark(Baseline = true)]
			public Array Classic() => data.Where(x => x > 8).Select(x => x.ToString()).ToArray();

			[Benchmark]
			public Array Current() => _current.Get(data);

			[Benchmark]
			public Array Proposed() => _subject.Get(data);
		}*/

		/*public class Benchmarks
		{
			readonly ISelect<int[], int[]> _subject;
			readonly ISelect<int[], int[]> _current;

			public Benchmarks() : this(new Start<int[], int>(A.Self<int[]>()).Get(new Build.Skip<int>(skip))
			                                                                 .Get(new Build.Take<int>(take))
			                                                                 .Get(),
			                           Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Array.By.Self.Query()
			                                .Skip(skip)
			                                .Take(take)
			                                .Out()) {}

			public Benchmarks(ISelect<int[], int[]> subject, ISelect<int[], int[]> current)
			{
				_subject = subject;
				_current = current;
			}

			[Benchmark(Baseline = true)]
			public Array Classic() => data.Skip(skip).Take(take).ToArray();

			[Benchmark]
			public Array Current() => _current.Get(data);

			[Benchmark]
			public Array Proposed() => _subject.Get(data);
		}*/
	}
}