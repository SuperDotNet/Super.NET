using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query.Temp;
using System.Collections.Generic;
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
			var sequence = new Shaped<int[], int>(A.Self<int[]>());
			sequence.Get(new Build.Skip<int>(skip))
			        .Get(new Build.Take<int>(take))
			        .Get(data)
			        .Should()
			        .Equal(data.Skip(skip).Take(take));

			new Build.Take<int>(take).Get(new Build.Skip<int>(skip).Get())
			                         .Select()
			                         .Get(data)
			                         .Should()
			                         .Equal(data.Skip(skip).Take(take));
		}

		[Fact]
		void VerifyWhere()
		{
			A.Self<int[]>()
			 .Select(Lease<int>.Default)
			 .Select(new Build.Where<int>(x => x > 8).Get().Select())
			 .Get(data)
			 .Should()
			 .Equal(data.Where(x => x > 8));
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
			readonly ISelect<int[], int[]> _subject;
			readonly IEnumerable<int>      _classic;

			public Benchmarks() : this( /*new Build.Take<int>(take).Get(new Build.Skip<int>(skip).Get())
			                                                     .Select()
			                                                     .To(Start.A.Selection.Of.Type<int>()
			                                                              .As.Sequence.Array.By
			                                                              .Instantiation<Store<int>>()
			                                                              .Select)*/
			                           A.Self<int[]>()
			                            .Select(Lease<int>.Default)
			                            .Select(new Build.Where<int>(x => x > 8).Get().Select())
			                           ,
			                           /*data.Skip(skip).Take(take)*/
			                           data.Where(x => x > 8)) {}

			public Benchmarks(ISelect<int[], int[]> subject, IEnumerable<int> classic)
			{
				_subject = subject;
				_classic = classic;
			}

			[Benchmark(Baseline = true)]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _subject.Get(data);
		}
	}
}