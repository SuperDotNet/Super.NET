using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
// ReSharper disable All

namespace Super.Testing.Runtime.Execution
{
	public sealed class AsyncLocalTests
	{
		readonly ITestOutputHelper _output;

		public AsyncLocalTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Expected()
		{
			/*/*ExecutionContext.Capture().Dispose();#1#
			var temp = SynchronizationContext.Current;

			Thread.Current*/

			new AsyncLocal<object>(args =>
			                       {
				                       if (args.ThreadContextChanged && args.CurrentValue == null)
				                       {
					                       _output.WriteLine("Invalidated!");
				                       }
			                       })
			{
				Value = new object()
			};
		}

		[Fact]
		Task ExpectedAsync() => Task.Run(action: Expected);

		/*sealed class ContextualExecution : IAlteration<Task>
		{
			public Func<Task> Get(Func<Task> parameter)
			{
				Task.Run()
				return null;
			}

			public Task Get(Task parameter)
			{
				return null;
			}
		}*/

		[Fact]
		Task UnexpectedAsync()
		{
			var origin = Thread.CurrentThread.ExecutionContext;
			Func<Task> function = async () =>
			                {
				                new AsyncLocal<object>(args =>
				                                       {
					                                       if (args.ThreadContextChanged && args.CurrentValue == null)
					                                       {
						                                       _output.WriteLine("Invalidated!");
					                                       }
				                                       }) {Value = new object()};
				                await Task.Yield();
			                };
			return Task.Run(function);
		}
	}
}