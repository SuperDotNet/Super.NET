using FluentAssertions;
using Super.Reflection;
using System;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public sealed class AttributeTests
	{
		[Fact]
		void Verify()
		{
			var provider = typeof(Extend);
			LocalAttribute<SubjectAttribute>.Default.Get(provider)
			                                .Should()
			                                .BeNull();

			var attribute = Attribute<SubjectAttribute>.Default.Get(provider);
			attribute.Should().NotBeNull();
			Attribute<SubjectAttribute>.Default.Get(provider).Should().BeSameAs(attribute);
		}

		sealed class Extend : Root {}

		[Subject]
		class Root {}

		[AttributeUsage(AttributeTargets.Class)]
		sealed class SubjectAttribute : Attribute {}
	}
}