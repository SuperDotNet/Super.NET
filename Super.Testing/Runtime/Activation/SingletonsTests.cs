﻿using FluentAssertions;
using JetBrains.Annotations;
using Super.Model.Instances;
using Super.Runtime.Activation;
using Xunit;

namespace Super.Testing.Runtime.Activation
{
	public class SingletonsTests
	{
		[Fact]
		public void Is() => Singletons.Default.Get(typeof(Subject))
		                                    .Should()
		                                    .BeSameAs(Subject.Default);

		[Fact]
		public void IsNot() => Singletons.Default.Get(typeof(Not))
		                                       .Should()
		                                       .BeNull();

		sealed class Subject : IInstance<int>
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
	}
}