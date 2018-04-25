using FluentAssertions;
using Super.Model.Selection;
using Super.Runtime.Activation;
using Xunit;

// ReSharper disable All

namespace Super.Testing.Application.Model.Sources
{
	public sealed class SelectionsTests
	{
		sealed class Count : IActivateMarker<int>
		{
			public static int Counter = 0;

			public Count(int number)
			{
				Number = number;
				Counter++;
			}

			public int Number { get; }
		}

		sealed class Other : IActivateMarker<int>
		{
			public static int Count = 0;

			public Other(int number)
			{
				Number = number;
				Count++;
			}

			public int Number { get; }
		}

		sealed class Subject<TParameter, TResult> : ISelect<TParameter, TResult>
		{
			public static Subject<TParameter, TResult> Default { get; } = new Subject<TParameter, TResult>();

			Subject() {}

			public TResult Get(TParameter parameter) => default;
		}

		[Fact]
		public void Verify()
		{
			var subject = Subject<string, int>.Default.ToDelegateReference();
			subject.Should()
			       .BeSameAs(Subject<string, int>.Default.ToDelegateReference());

			subject.Return().Should().BeSameAs(subject.Return());
		}
	}
}