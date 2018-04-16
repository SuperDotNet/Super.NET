using Super.ExtensionMethods;
using Super.Text;

namespace Super.Runtime.Execution {
	sealed class TaskDetailsFormatter : IFormatter<TaskDetails>
	{
		public static TaskDetailsFormatter Default { get; } = new TaskDetailsFormatter();

		TaskDetailsFormatter() {}

		public string Get(TaskDetails parameter)
			=> $"Task: {parameter.TaskId.OrNone()}, Default/Current Scheduler: {parameter.Default.Id.ToString()}/{parameter.Current.Id.ToString()}";
	}
}