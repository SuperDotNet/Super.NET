using FluentAssertions;
using Super.Compose;
using Super.Model.Sequences;
using Super.Model.Sequences.Query.Temp;
using System.Linq;
using Xunit;

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
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Skip<int>(skip))
			                                      .Get(new Build.Take<int>(take))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Take(take))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();

			new Build.Take<int>(take).Get(new Build.Skip<int>(skip).Get())
			                         .Select()
			                         .Get(data)
			                         .Should()
			                         .Equal(data.Skip(skip).Take(take))
			                         .And.Subject.Should()
			                         .NotBeEmpty();
		}

		[Fact]
		void VerifySkipTakeWhere()
		{
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Skip<int>(skip))
			                                      .Get(new Build.Take<int>(take))
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
			new Start<int[], int>(A.Self<int[]>()).Get(new Build.Skip<int>(skip))
			                                      .Get(new Build.Where<int>(x => x == 5))
			                                      .Get(new Build.Take<int>(take))
			                                      .Get(data)
			                                      .Should()
			                                      .Equal(data.Skip(skip).Where(x => x == 5).Take(take))
			                                      .And.Subject.Should()
			                                      .NotBeEmpty();
		}

		[Fact]
		void VerifyWhere()
		{
			A.Self<int[]>()
			 .Select(Lease<int>.Default)
			 .Select(new Build.Where<int>(x => x > 8).Get().Select())
			 .Get(data)
			 .Should()
			 .Equal(data.Where(x => x > 8))
			 .And.Subject.Should()
			 .NotBeEmpty();

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
			     .As.Sequence.Array.By.StoredActivation<Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Instantiation<Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);
			//Enumerable
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
			/*readonly ISelect<int[], string[]> _subject;
			readonly ISelect<int[], string[]> _other;
			readonly IEnumerable<string>      _classic;

			public Benchmarks() : this(new Start<int[], int>(A.Self<int[]>())
			                           .Get(new Build.Where<int>(x => x > 8))
			                           .Get(new Build.Select<int, string>(x => x.ToString()))
			                           .Get(),
			                           Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Array.By.Self.Query()
			                                .Where(x => x > 8)
			                                .Select(x => x.ToString())
			                                .Out(),
			                           data.Where(x => x > 8).Select(x => x.ToString())) {}

			public Benchmarks(ISelect<int[], string[]> subject, ISelect<int[], string[]> other,
			                  IEnumerable<string> classic)
			{
				_subject = subject;
				_other   = other;
				_classic = classic;
			}

			[Benchmark(Baseline = true)]
			public string[] Classic() => data.Where(x => x > 8).Select(x => x.ToString()).ToArray();

			[Benchmark]
			public string[] Other() => _other.Get(data);*/

			/*[Benchmark]
			public string[] Subject() => _subject.Get(data);*/
		}
	}
}