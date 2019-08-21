using Super.Runtime.Execution;
using Super.Text;
using System.Threading;

namespace Super.Diagnostics.Logging.Text
{
	sealed class ThreadingDetailsFormatter : IFormatter<ThreadingDetails>
	{
		public static ThreadingDetailsFormatter Default { get; } = new ThreadingDetailsFormatter();

		ThreadingDetailsFormatter() : this(ThreadFormatter.Default) {}

		readonly IFormatter<Thread> _thread;

		public ThreadingDetailsFormatter(IFormatter<Thread> thread) => _thread = thread;

		public string Get(ThreadingDetails parameter)
			=> $"Thread: {_thread.Get(parameter.Thread)}, SynchronizationContext: {parameter.Synchronization.OrNone()}";
	}
}