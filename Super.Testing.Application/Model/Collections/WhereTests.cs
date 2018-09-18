// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{
		/*readonly static string[] Strings = Objects.Data.Default.Get();

		readonly ISelect<Unit, View<int>> _load = Strings.ToSource()
		                                                 .Out()
		                                                 .AsSelect()
		                                                 .Iterate()
		                                                 .Selection(x => x.Length);*/
		/*[Fact]
		void Builder()
		{
			var total = 100_000u;
			var data = Objects.Count.Default.Get(total);
			var builder = new ArrayBuilder<int>(total - 5u, 5);
			var iterator = new ArrayIterator<int>(data);
			var array = builder.Get(iterator);

			Objects.Count.Default.Get(total).Skip((int)(total - 5)).Take(5).ToArray().Should().Equal(array);
		}

		[Theory, AutoData]
		void Select(IEnumerable<string> strings)
		{
			var total    = 100_000u;
			var data     = strings.Take((int)total).ToArray();
			var builder  = new ArrayBuilder<int>(0, total);
			var iterator = new Iterator<string,int>(new ArrayIterator<string>(data), x => x.Length);
			var array    = builder.Get(iterator);

			data.Select(x => x.Length).ToArray().Should().Equal(array);
		}*/

		readonly ITestOutputHelper _output;

		public WhereTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Output()
		{
			_output.WriteLine($" {Unsafe.SizeOf<Lease<int>>()} - {Unsafe.SizeOf<View<int>>()}");
		}

		struct View<T>
		{
			public View(Lease<T> lease) => Lease = lease;

			public Lease<T> Lease { get; }
		}

		public struct Lease<T>
		{
			public Lease(T[] reference, uint requested, uint actual)
			{
				Reference = reference;
				Requested = requested;
				Actual    = actual;
			}

			public T[] Reference { get; }

			public uint Requested { get; }

			public uint Actual { get; }
		}

		[Fact]
		void Verify()
		{
			/*var expected = Enumerable.Range(0, 10_000).Select(x => x).ToArray();

			var buffer  = new Buffer<int>(10);
			var current = buffer;
			for (var i = 0; i < expected.Length; i++)
			{
				current.Append(i);
			}
			current.Flush().Should().Equal(expected);*/
			/*Objects.Data.Default.Get().ToSource()
			       .Out()
			       .AsSelect()
			       .Iterate().Where(x => x.Length > 1000)
			       .Get(Unit.Default)
			       .Allocate();*/
			const uint count = 10_000_000u;
			var array = Objects.Count.Default
			                   .Iterate()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Get(count)
			                   .Get()
			                   ;
			array.Should().HaveCount(5);


			Objects.Count.Default.Get(count).Skip((int)(count - 5)).Take(5).Sum().Should().Be(array.Sum());

			/*var segment = Objects.Count.Default
			                          .Iterate()
			                          .Selection(x => x + 1)
			                          .Get(10_000);
			segment.Count.Should().Be(10_000);

			segment.ToArray().Sum(x => x).Should().Be(Objects.Count.Default.Get(10_000).Sum(x => x) + 10_000);*/
		}


	}
}