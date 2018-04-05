/*using Super.Application.Console;
using System.Collections.Immutable;
using Xunit;

namespace Super.Testing.Application.Console
{
	public sealed class ConsoleApplicationContextsTests
	{
		[Fact]
		void Verify1()
		{
			ConsoleApplicationContexts<Program>.Default.Get(ImmutableArray<string>.Empty);

			/*var temp = CallContext.HostContext;
			var asdf = ExecutionContext.Capture();

			var two = TaskScheduler.Default;
			var thr = TaskScheduler.Current;

			Debugger.Break();#1#
		}

/*
		[Fact]
		async Task Verify()
		{
			/*var temp = CallContext.HostContext;
			var asdf0 = ExecutionContext.Capture();
			var asdf01 = ExecutionContext.Capture();


			//var context = ConsoleApplicationContexts<Program>.Default.Get(ImmutableArray<string>.Empty);
			var onea = SynchronizationContext.Current;

			var one = TaskScheduler.FromCurrentSynchronizationContext();
			var two = TaskScheduler.Default;
			var thr = TaskScheduler.Current;

			await Task.Yield();

			var t1emp = CallContext.HostContext;

			var ona = SynchronizationContext.Current;
			var asdf = ExecutionContext.Capture();
			var asdf1 = ExecutionContext.Capture();
			var one1 = TaskScheduler.FromCurrentSynchronizationContext();
			var two1 = TaskScheduler.Default;
			var thr1 = TaskScheduler.Current;

			Debugger.Break();#2#
		}
#1#

		sealed class Program : IConsoleApplication
		{
			public void Execute(ImmutableArray<string> parameter) {}
		}
	}
}*/