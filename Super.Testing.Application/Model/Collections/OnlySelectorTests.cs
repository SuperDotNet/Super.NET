using AutoFixture;
using FluentAssertions;
using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class OnlySelectorTests
	{
		[Fact]
		void Verify()
		{
			var data = new Fixture().CreateMany<string>(100).ToArray();
			var selected = new ArraySelectInline<string, int>(x => x.Length).Get(new Array<string>(data));
			data.Select(x => x.Length).Should().Equal(selected._source);
		}



		public sealed class OnlySelector<T> : ISelect<ImmutableArray<T>, T>
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
		}
	}
}