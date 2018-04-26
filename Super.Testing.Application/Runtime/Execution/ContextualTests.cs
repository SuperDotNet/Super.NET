using FluentAssertions;
using Super.Runtime.Execution;
using Super.Testing.Objects;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class ContextualTests
	{
		[Fact]
		void VerifyResource()
		{
			AssignedContext.Default.IsSatisfiedBy().Should().BeFalse();
			AssignedContext.Default.Get().Should().BeNull();
			var instance = Resource.Default.Get();
			var context = AssignedContext.Default.Get();
			context.Should().NotBeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeTrue();
			instance.Get().Should().Be(0);
			AssignedContext.Default.IsSatisfiedBy().Should().BeTrue();
			DisposeContext.Default.Execute();

			instance.Get().Should().Be(1);
			AssignedContext.Default.Get().Should().BeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeFalse();

			DisposeContext.Default.Execute();
			instance.Get().Should().Be(1);
			AssignedContext.Default.Get().Should().BeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeFalse();
		}

		sealed class Resource : Contextual<CountingDisposable>
		{
			public static Resource Default { get; } = new Resource();

			Resource() : base(() => new CountingDisposable()) {}
		}
	}
}