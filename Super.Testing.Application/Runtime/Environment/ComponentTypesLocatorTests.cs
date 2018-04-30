using FluentAssertions;
using Super.Model.Collections;
using Super.Reflection.Selection;
using Super.Runtime.Environment;
using Super.Runtime.Execution;
using System;
using System.Linq;
using Xunit;
// ReSharper disable All

namespace Super.Testing.Application.Runtime.Environment
{
	public sealed class ComponentTypesTests
	{
		[Fact]
		void Verify()
		{
			SortSelector<Type>.Default.Get(typeof(First)).Should().Be(-10);

			Types.Default.Execute(new NestedTypes<ComponentTypesTests>().Get());

			var types = ComponentTypes.Default.Get(typeof(IComponent));
			types.Should().HaveCount(4);
			types.Should().BeEquivalentTo(typeof(First), typeof(Subject), typeof(AnotherSubject), typeof(Last));
			types.First().Should().Be(typeof(First));
			types.Last().Should().Be(typeof(Last));
		}

		[Fact]
		void VerifyCount()
		{
			var callBefore = new Counter();
			var callAfter  = new Counter();

			var before = new Counter();
			var after = new Counter();
			var sut = Start.New(() => In<Type>.Select(x => new object()))
			                                  .Select(x => x.Select(callBefore.ExecuteAndReturn)
			                                                .ToStore()
			                                                .Select(callAfter.ExecuteAndReturn))
			                                  .Select(before.ExecuteAndReturn)
			                                  .ToContextual()
			                                  .Select(after.ExecuteAndReturn);

			callBefore.Get().Should().Be(0);
			callAfter.Get().Should().Be(0);
			before.Get().Should().Be(0);
			after.Get().Should().Be(0);
			var first = sut.Get();

			callBefore.Get().Should().Be(0);
			callAfter.Get().Should().Be(0);
			before.Get().Should().Be(1);
			after.Get().Should().Be(1);

			var second = sut.Get();

			first.Should().BeSameAs(second);

			callBefore.Get().Should().Be(0);
			callAfter.Get().Should().Be(0);
			before.Get().Should().Be(1);
			after.Get().Should().Be(2);

			var response = second.Get(typeof(IUnused)).Should().NotBeNull().And.Subject;
			callBefore.Get().Should().Be(1);
			callAfter.Get().Should().Be(1);
			before.Get().Should().Be(1);
			after.Get().Should().Be(2);

			second.Get(typeof(IUnused)).Should().BeSameAs(response);
			callBefore.Get().Should().Be(1);
			callAfter.Get().Should().Be(2);
			before.Get().Should().Be(1);
			after.Get().Should().Be(2);

			second.Get(typeof(IOther)).Should().Should().NotBeNull().And.Subject.Should().NotBeSameAs(response);
			callBefore.Get().Should().Be(2);
			callAfter.Get().Should().Be(3);
			before.Get().Should().Be(1);
			after.Get().Should().Be(2);
		}

		public interface IUnused {}

		public interface IOther {}

		public interface IComponent {}

		sealed class Subject : IComponent {}

		sealed class AnotherSubject : IComponent {}

		sealed class NotSubject {}

		[Sort(-10)]
		sealed class First : IComponent{}

		sealed class Last : IComponent, ISortAware {
			public int Get()
			{
				return 100;
			}
		}
	}
}