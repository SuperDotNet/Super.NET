using AutoFixture;
using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using Xunit;
// ReSharper disable ComplexConditionExpression

namespace Super.Testing.Application.Model.Collections
{
	public sealed class OnlySelectorTests
	{
		[Fact]
		void Verify()
		{
			var data = new Fixture().CreateMany<string>(100).ToArray();
			var selected = new ArraySelectInline<string, int>(x => x.Length).Get(new Array<string>(data));
			data.Select(x => x.Length).Should().Equal(selected.Get());
		}

		[Fact]
		void VerifyInline()
		{
			var sut = new ArraySelectInline<string, string>(x => x + "Hello!");
			var data = new[] {"One", "Two", "Three"};
			var selected = sut.Get(new Array<string>(data));
			selected.Get().Should().Equal("OneHello!", "TwoHello!", "ThreeHello!");
		}

		[Fact]
		void VerifyMultipleInline()
		{
			var sut      = new ArraySelectInline<string, string>(x => x + "Hello!" + x.Length);
			var data     = new[] {"One", "Two", "Three"};
			var selected = sut.Get(new Array<string>(data));
			selected.Get().Should().Equal("OneHello!3", "TwoHello!3", "ThreeHello!5");
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