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
			ExecutionContextStore.Default.IsSatisfiedBy().Should().BeFalse();
			var instance = Resource.Default.Get();
			var context = ExecutionContextStore.Default.Get();
			context.Should().NotBeNull();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeTrue();
			instance.Get().Should().Be(0);
			ExecutionContextStore.Default.IsSatisfiedBy().Should().BeTrue();
			DisposeContext.Default.Execute();

			instance.Get().Should().Be(1);
			ExecutionContextStore.Default.IsSatisfiedBy().Should().BeFalse();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeFalse();

			DisposeContext.Default.Execute();
			instance.Get().Should().Be(1);
			ExecutionContextStore.Default.IsSatisfiedBy().Should().BeFalse();
			AssociatedResources.Default.IsSatisfiedBy(context).Should().BeFalse();
		}

		sealed class Resource : Contextual<CountingDisposable>
		{
			public static Resource Default { get; } = new Resource();

			Resource() : base(() => new CountingDisposable()) {}
		}
	}
}