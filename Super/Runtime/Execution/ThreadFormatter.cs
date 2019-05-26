using System.Threading;
using Super.Text;

namespace Super.Runtime.Execution
{
	sealed class ThreadFormatter : IFormatter<Thread>
	{
		public static ThreadFormatter Default { get; } = new ThreadFormatter();

		ThreadFormatter() {}

		public string Get(Thread parameter)
			=> $"#{parameter.ManagedThreadId.ToString()} {parameter.Priority.ToString()} {parameter.Name.OrNone()}";
	}
}