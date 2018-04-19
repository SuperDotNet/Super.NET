using AutoFixture.Xunit2;
using FluentAssertions;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Runtime
{
	public sealed class ContextFormatterTests
	{
		[Theory, Super.Application.Testing.AutoData]
		void Verify(ContextFormatter sut, [Greedy]Context context)
		{
			var thread = context.Threading.Thread;
			sut.Get(context)
			   .Should()
			   .Be($"[11:18:24:000] {context.Details.Name}: Task: {context.Task.TaskId.OrNone()}, Default/Current Scheduler: {context.Task.Default.Id}/{context.Task.Default.Id}, Thread: #{thread.ManagedThreadId} {thread.Priority} {thread.Name ?? thread.CurrentCulture.DisplayName}, SynchronizationContext: {context.Threading.Synchronization.OrNone()}");
		}
	}
}