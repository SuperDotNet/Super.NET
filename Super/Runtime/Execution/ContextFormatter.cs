﻿using Super.Text;

namespace Super.Runtime.Execution {
	sealed class ContextFormatter : IFormatter<Context>
	{
		public static ContextFormatter Default { get; } = new ContextFormatter();

		ContextFormatter() : this(DetailsFormatter.Default, TaskDetailsFormatter.Default,
		                          ThreadingDetailsFormatter.Default) {}

		readonly IFormatter<Details>          _details;
		readonly IFormatter<TaskDetails>      _task;
		readonly IFormatter<ThreadingDetails> _thread;

		public ContextFormatter(IFormatter<Details> details, IFormatter<TaskDetails> task,
		                        IFormatter<ThreadingDetails> thread)
		{
			_details = details;
			_task    = task;
			_thread  = thread;
		}

		public string Get(Context parameter)
			=> $"{_details.Get(parameter.Details)}: {_task.Get(parameter.Task)}, {_thread.Get(parameter.Threading)}";
	}
}