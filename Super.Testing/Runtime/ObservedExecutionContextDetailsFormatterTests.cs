using FluentAssertions;
using Super.Application.Testing;
using Super.Diagnostics.Logging;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Runtime
{
	public sealed class ContextFormatterTests
	{
		[Theory, AutoData]
		void Verify(ContextFormatter sut, ContextDetails details)
		{
			var thread = details.Threading.Thread;
			sut.Get(details)
			   .Should()
			   .Be($"[{TimestampFormatter.Default.Get(Epoch.Default)}] {details.Details.Name}: Task: {details.Task.TaskId.OrNone()}, Default/Current Scheduler: {details.Task.Default.Id}/{details.Task.Default.Id}, Thread: #{thread.ManagedThreadId} {thread.Priority} {thread.Name ?? thread.CurrentCulture.DisplayName}, SynchronizationContext: {details.Threading.Synchronization.OrNone()}");
		}
	}
}