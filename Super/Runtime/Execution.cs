using Super.Diagnostics.Logging;
using Super.ExtensionMethods;
using Super.Runtime.Activation;
using Super.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Runtime
{
	sealed class DetailsFormatter : IFormatter<Details>
	{
		public static DetailsFormatter Default { get; } = new DetailsFormatter();

		DetailsFormatter() {}

		public string Get(Details parameter)
			=> $"[{parameter.Observed.ToString(TimestampFormat.Default)}] {parameter.Name}";
	}

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

	sealed class ThreadingDetailsFormatter : IFormatter<ThreadingDetails>
	{
		public static ThreadingDetailsFormatter Default { get; } = new ThreadingDetailsFormatter();

		ThreadingDetailsFormatter() : this(ThreadFormatter.Default) {}

		readonly IFormatter<Thread> _thread;

		public ThreadingDetailsFormatter(IFormatter<Thread> thread) => _thread = thread;

		public string Get(ThreadingDetails parameter)
			=> $"Thread: {_thread.Get(parameter.Thread)}, SynchronizationContext: {parameter.Synchronization.OrNone()}";
	}

	sealed class ThreadFormatter : IFormatter<Thread>
	{
		public static ThreadFormatter Default { get; } = new ThreadFormatter();

		ThreadFormatter() {}

		public string Get(Thread parameter)
			=> $"#{parameter.ManagedThreadId.ToString()} {parameter.Priority.ToString()} {parameter.Name ?? parameter.CurrentCulture.DisplayName}";
	}

	sealed class TaskDetailsFormatter : IFormatter<TaskDetails>
	{
		public static TaskDetailsFormatter Default { get; } = new TaskDetailsFormatter();

		TaskDetailsFormatter() {}

		public string Get(TaskDetails parameter)
			=> $"Task: {parameter.TaskId.OrNone()}, Default/Current Scheduler: {parameter.Default.Id.ToString()}/{parameter.Current.Id.ToString()}";
	}

	sealed class TaskDetails
	{
		public TaskDetails() : this(TaskScheduler.Default, TaskScheduler.Current, Task.CurrentId) {}

		public TaskDetails(TaskScheduler @default, TaskScheduler current, int? taskId)
		{
			Default = @default;
			Current = current;
			TaskId  = taskId;
		}

		public TaskScheduler Default { get; }

		public TaskScheduler Current { get; }

		public int? TaskId { get; }
	}

	sealed class ThreadingDetails
	{
		public ThreadingDetails() : this(SynchronizationContext.Current, Thread.CurrentThread) {}

		public ThreadingDetails(SynchronizationContext synchronization, Thread thread)
		{
			Synchronization = synchronization;
			Thread          = thread;
		}

		public SynchronizationContext Synchronization { get; }

		public Thread Thread { get; }
	}

	public struct Details
	{
		public Details(string name, DateTimeOffset observed)
		{
			Name     = name;
			Observed = observed;
		}

		public string Name { get; }

		public DateTimeOffset Observed { get; }
	}

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

	/*class Execution<T> : Contextual<IIExecutionContext, T>
	{
		public Execution(IInstance<IIExecutionContext> parameter) : base(parameter) {}

		public Execution(ISource<IIExecutionContext, T> source, IInstance<IIExecutionContext> parameter) : base(source, parameter) {}
	}

	class Contextual<TContext, T> : DelegatedParameterSource<TContext, T> where TContext : class
	{
		public Contextual(IInstance<TContext> parameter) : this(null, parameter) {}

		public Contextual(ISource<TContext, T> source, IInstance<TContext> parameter) : base(source, parameter) {}
	}*/

	/*sealed class ExecutionContext : Decorated<IExecutionContextInformation>
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContexts.Default.Select(x => x.Peek().Get())) {}
	}

	sealed class RootExecutionContext : ExecutionContext<AppDomain>
	{
		public static RootExecutionContext Default { get; } = new RootExecutionContext();

		RootExecutionContext()
			: this() {}

		public RootExecutionContext(string message)
			: base(RootExecutionContextInformation.Default, message.New(I<InvalidOperationException>.Default)
			                                                       .New(I<ThrowCommand<InvalidOperationException>>.Default)
			                                                       .Execute) {}
	}*/

	/*public interface IExecutionContextInformation
	{
		string Name { get; }

		/*IExecutionContextInformation Parent { get; }#1#

		object Reference { get; }
	}

	sealed class RootExecutionContextInformation : ExecutionContextInformation<AppDomain>
	{
		public static RootExecutionContextInformation Default { get; } = new RootExecutionContextInformation();

		RootExecutionContextInformation() : base("Root Execution Context", null, AppDomain.CurrentDomain) {}
	}

	public class ExecutionContextInformation<T> : IExecutionContextInformation where T : class
	{
		protected ExecutionContextInformation(string name, IExecutionContextInformation parent, T reference)
		{
			Name      = name;
			Parent    = parent;
			Reference = reference;
		}

		public string Name { get; }

		public IExecutionContextInformation Parent { get; }

		public T Reference { get; }

		object IExecutionContextInformation.Reference => Reference;
	}*/
}