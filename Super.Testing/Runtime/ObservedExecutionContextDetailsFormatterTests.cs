using FluentAssertions;
using Super.ExtensionMethods;
using Super.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Super.Testing.Runtime
{
	public sealed class ObservedExecutionContextDetailsFormatterTests
	{
		[Theory, Super.Application.Testing.AutoData]
		void Verify(ObservedExecutionContextDetailsFormatter sut, ObservedExecutionContextDetails details)
		{
			sut.Get(details)
			   .Should()
			   .Be($"[11:18:24:000] Task: {Task.CurrentId.OrNone()}, Default/Current Scheduler: {TaskScheduler.Default.Id}/{TaskScheduler.Current.Id}, Thread: #{Thread.CurrentThread.ManagedThreadId} {Thread.CurrentThread.Priority} {Thread.CurrentThread.Name ?? Thread.CurrentThread.CurrentCulture.DisplayName}, SynchronizationContext: {details.Threading.Synchronization.OrNone()}");
		}
	}
}