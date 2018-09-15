using AutoFixture;
using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using Xunit;
// ReSharper disable ComplexConditionExpression

namespace Super.Testing.Application.Model.Collections
{
	public sealed class OnlyElementTests
	{
		[Fact]
		void Verify()
		{
			var data = new View<string>(new Fixture().CreateMany<string>(100).ToArray());
			var selected = new Selection<string, int>(x => x.Length).Get(data);
			data.Allocate().Select(x => x.Length).Should().Equal(selected.Allocate());
		}

		[Fact]
		void VerifyInline()
		{
			var sut = new Selection<string, string>(x => x + "Hello!");
			var data = new View<string>("One", "Two", "Three");
			var selected = sut.Get(data);
			selected.Allocate().Should().Equal("OneHello!", "TwoHello!", "ThreeHello!");
		}

		[Fact]
		void VerifyMultipleInline()
		{
			var sut      = new Selection<string, string>(x => x + "Hello!" + x.Length);
			var data = new View<string>("One", "Two", "Three");
			var selected = sut.Get(data);
			selected.Allocate().Should().Equal("OneHello!3", "TwoHello!3", "ThreeHello!5");
		}


		/*public sealed class OnlySelector<T> : ISelect<ImmutableArray<T>, T>
		{
			public static OnlySelector<T> Default { get; } = new OnlySelector<T>();

			OnlySelector() : this(x => true) {}

			readonly Func<T, bool> _where;

			public OnlySelector(Func<T, bool> where) => _where = where;

			public T Get(ImmutableArray<T> parameter)
			{
				var enumerable = parameter.Where(_where).ToArray();
				var result     = enumerable.Length == 1 ? enumerable[0] : default;
				return result;
			}
		}*/
	}
}