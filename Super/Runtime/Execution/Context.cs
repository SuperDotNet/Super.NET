using System;
using Super.Runtime.Activation;

namespace Super.Runtime.Execution {
	sealed class Context : IActivateMarker<string>
	{
		public Context(string name) : this(name, Time.Default.Get()) {}

		public Context(string name, DateTimeOffset observed)
			: this(new Details(name, observed), new ThreadingDetails(), new TaskDetails()) {}

		public Context(Details details, ThreadingDetails threading, TaskDetails task)
		{
			Details   = details;
			Threading = threading;
			Task      = task;
		}

		public Details Details { get; }
		public ThreadingDetails Threading { get; }
		public TaskDetails Task { get; }
	}
}