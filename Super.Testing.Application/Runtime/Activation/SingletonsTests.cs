using FluentAssertions;
using JetBrains.Annotations;
using Super.Model.Sources;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Application.Runtime.Activation
{
	public class SingletonsTests
	{
		sealed class Subject : ISource<int>
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get() => 6776;
		}

		sealed class Not
		{
			[UsedImplicitly]
			public object InvalidName { get; set; }
		}

		[Fact]
		public void Is() => Singletons.Default.Get(typeof(Subject))
		                              .Should()
		                              .BeSameAs(Subject.Default);

		[Fact]
		public void IsNot() => Singletons.Default.Get(typeof(Not))
		                                 .Should()
		                                 .BeNull();
	}
}