using FluentAssertions;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Xunit;

namespace Super.Testing.Model.Sources
{
	public sealed class SourcesTests
	{
		[Fact]
		public void Verify()
		{
			var subject = Subject<string, int>.Default.ToDelegate();
			subject.Should()
			       .BeSameAs(Subject<string, int>.Default.ToDelegate());

			subject.ToSource().Should().BeSameAs(subject.ToSource());
		}

		sealed class Subject<TParameter, TResult> : ISource<TParameter, TResult>
		{
			public static Subject<TParameter, TResult> Default { get; } = new Subject<TParameter, TResult>();
			Subject() {}

			public TResult Get(TParameter parameter) => default;
		}
	}
}