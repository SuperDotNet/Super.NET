using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Runtime
{
	sealed class ObservedExecutionContext : ExecutionContext<ObservedExecutionContextDetails>
	{
		public ObservedExecutionContext(IExecutionContextInformation parent, Action dispose)
			: this(new ObservedExecutionContextInformation(parent), dispose) {}

		public ObservedExecutionContext(ObservedExecutionContextInformation instance, Action dispose) :
			base(instance, dispose) {}
	}

	sealed class ObservedExecutionContextInformation : ExecutionContextInformation<ObservedExecutionContextDetails>
	{
		public ObservedExecutionContextInformation(IExecutionContextInformation parent)
			: this(parent, Time.Default) {}

		public ObservedExecutionContextInformation(IExecutionContextInformation parent, ITime time)
			: this(parent, new ObservedExecutionContextDetails(time.Get())) {}

		public ObservedExecutionContextInformation(IExecutionContextInformation parent,
		                                           ObservedExecutionContextDetails reference)
			: this(ObservedExecutionContextDetailsFormatter.Default.Get(reference), parent, reference) {}

		public ObservedExecutionContextInformation(string name, IExecutionContextInformation parent,
		                                           ObservedExecutionContextDetails reference)
			: base(name, parent, reference) {}
	}

	sealed class ObservedExecutionContextDetailsFormatter : IFormatter<ObservedExecutionContextDetails>
	{
		public static ObservedExecutionContextDetailsFormatter Default { get; } =
			new ObservedExecutionContextDetailsFormatter();

		ObservedExecutionContextDetailsFormatter() {}

		public string Get(ObservedExecutionContextDetails parameter)
			=> $"{parameter.Observed.ToString()} - Task: {parameter.Task.TaskId?.ToString()}, Thread: {parameter.Threading.Thread.Name} ({parameter.Threading.Thread.ManagedThreadId.ToString()}), Default/Current Scheduler: {parameter.Task.Default.Id.ToString()}/{parameter.Task.Current.Id.ToString()}, SynchronizationContext: {parameter.Threading.Synchronization}";
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

	sealed class ObservedExecutionContextDetails
	{
		public ObservedExecutionContextDetails(DateTimeOffset observed)
			: this(new ThreadingDetails(), new TaskDetails(), observed) {}

		public ObservedExecutionContextDetails(ThreadingDetails threading, TaskDetails task, DateTimeOffset observed)
		{
			Threading = threading;
			Task      = task;
			Observed  = observed;
		}

		public ThreadingDetails Threading { get; }
		public TaskDetails Task { get; }
		public DateTimeOffset Observed { get; }
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

	public interface IIExecutionContext : ISource<IExecutionContextInformation>, IDisposable {}

	class ExecutionContext<T> : Source<IExecutionContextInformation>, IIExecutionContext where T : class
	{
		readonly Action _dispose;

		protected ExecutionContext(ExecutionContextInformation<T> instance, Action dispose) : base(instance)
			=> _dispose = dispose;

		public void Dispose()
		{
			_dispose();
		}
	}

	sealed class Observe : IAlteration<IIExecutionContext>
	{
		public static Observe Default { get; } = new Observe();

		Observe() {}

		public IIExecutionContext Get(IIExecutionContext parameter) => new ObservedExecutionContext(parameter.Get(), () => {});
	}

	sealed class ExecutionContextStack : Stack<IIExecutionContext>
	{
		[UsedImplicitly]
		public ExecutionContextStack() : this(RootExecutionContext.Default) {}

		public ExecutionContextStack(IIExecutionContext root) : this(root, root.To(Observe.Default)) {}

		public ExecutionContextStack(params IIExecutionContext[] contexts) : this(contexts.AsEnumerable()) {}

		public ExecutionContextStack(IEnumerable<IIExecutionContext> collection) : base(collection) {}
	}

	sealed class ExecutionContext : Delegated<IExecutionContextInformation>
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(() => ExecutionContexts.Default.Get().Peek().Get()) {}
	}

	sealed class RootExecutionContext : ExecutionContext<AppDomain>
	{
		public static RootExecutionContext Default { get; } = new RootExecutionContext();

		RootExecutionContext() :
			this("An attempt was made to dispose of the root execution context, which is not allowed.") {}

		public RootExecutionContext(string message)
			: base(RootExecutionContextInformation.Default, message.New(I<InvalidOperationException>.Default)
			                                                       .New(I<ThrowCommand<InvalidOperationException>>.Default)
			                                                       .Execute) {}
	}

	public interface IExecutionContextInformation
	{
		string Name { get; }

		IExecutionContextInformation Parent { get; }

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
	}
}