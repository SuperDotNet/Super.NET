using Polly;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Execution;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Runtime.Invocation.Operations
{
	class EventSubscriber : EventSubscriber<EventArgs>
	{
		public EventSubscriber(ICommand<EventHandler> add, ICommand<EventHandler> remove)
			: this(In<EventHandler<EventArgs>>.New<EventHandler>(), add, remove) {}

		public EventSubscriber(ISelect<EventHandler<EventArgs>, EventHandler> select, ICommand<EventHandler> add,
		                       ICommand<EventHandler> remove)
			: base(select.Enter(add), select.Enter(remove)) {}
	}

	class EventSubscriber<T> : Subscriber<EventPattern<T>> where T : EventArgs
	{
		public EventSubscriber(ICommand<EventHandler<T>> add, ICommand<EventHandler<T>> remove)
			: this(add.Execute, remove.Execute) {}

		public EventSubscriber(Action<EventHandler<T>> add, Action<EventHandler<T>> remove)
			: this(Observable.FromEventPattern(add, remove)) {}

		public EventSubscriber(IObservable<EventPattern<T>> instance) : base(instance) {}
	}

	sealed class DomainUnload : EventSubscriber
	{
		public static DomainUnload Default { get; } = new DomainUnload();

		DomainUnload() : this(AppDomain.CurrentDomain) {}

		public DomainUnload(AppDomain domain) : this(new RegisterUnload(domain), new UnregisterUnload(domain)) {}

		public DomainUnload(ICommand<EventHandler> add, ICommand<EventHandler> remove) : base(add, remove) {}

		sealed class RegisterUnload : ICommand<EventHandler>
		{
			readonly AppDomain _domain;

			public RegisterUnload(AppDomain domain) => _domain = domain;

			public void Execute(EventHandler parameter)
			{
				_domain.DomainUnload += parameter;
			}
		}

		sealed class UnregisterUnload : ICommand<EventHandler>
		{
			readonly AppDomain _domain;

			public UnregisterUnload(AppDomain domain) => _domain = domain;

			public void Execute(EventHandler parameter)
			{
				_domain.DomainUnload -= parameter;
			}
		}
	}

	class Subscriber<T> : Source<IObservable<T>>
	{
		public Subscriber(IObservable<T> instance) : base(instance) {}
	}

	public interface IObserve<T> : ISelect<IObservable<T>, T> {}

	public class DurableObservableSource<T> : IObserve<T>
	{
		readonly Func<Func<T>, T> _policy;

		public DurableObservableSource(ISyncPolicy policy) : this(policy.Execute) {}

		public DurableObservableSource(ISyncPolicy<T> policy) : this(policy.Execute) {}

		public DurableObservableSource(Func<Func<T>, T> policy) => _policy = policy;

		public T Get(IObservable<T> parameter) => _policy(parameter.Wait);
	}

	sealed class TaskSelector : Select<Action, Task>
	{
		public static TaskSelector Default { get; } = new TaskSelector();

		TaskSelector() : base(Task.Run) {}
	}

	sealed class TaskSelector<T> : Select<Func<T>, Task<T>>
	{
		public static TaskSelector<T> Default { get; } = new TaskSelector<T>();

		TaskSelector() : base(Task.Run) {}
	}

	sealed class InvokeTaskSelector : Invoke<Task>
	{
		public static InvokeTaskSelector Default { get; } = new InvokeTaskSelector();

		InvokeTaskSelector() : base(Task.Run) {}
	}

	/*sealed class ContinuationSelector : ISelect<Task, Task>
	{
		readonly Action<Task, object> _continuation;
		readonly Func<OperationState> _state;

		public ContinuationSelector(Action<Task, object> continuation, Func<OperationState> state)
		{
			_continuation = continuation;
			_state = state;
		}

		public Task Get(Task parameter)
		{
			var state = _state();
			var result = parameter.ContinueWith(_continuation, state, state.Token);
			return result;
		}
	}*/

	sealed class OperationContext : Logical<OperationState>
	{
		public static OperationContext Default { get; } = new OperationContext();

		OperationContext() {}
	}

	/*class Operation<T> : ISelect<T, Task>
	{
		readonly Func<T, Task> _task;
		readonly Func<string, IDisposable> _context;
		//readonly Func<OperationState> _state;

		public Operation(Func<T, Task> task, Func<string, IDisposable> context)
		{
			_task = task;
			_context = context;

		}

		public Task Get(T parameter)
		{
			/*var context = _context(string.Empty);
			/*var state = new Logical<OperationState>(new OperationState(""));#2#
			var result = _task(parameter).ContinueWith()
			return result;#1#
			return null;
		}
	}*/

	sealed class OperationState
	{
		public OperationState(string name, CancellationToken token = new CancellationToken())
			: this(new ContextDetails(name), token) {}

		public OperationState(ContextDetails contextDetails, CancellationToken token = new CancellationToken())
		{
			ContextDetails = contextDetails;
			Token          = token;
		}

		public ContextDetails ContextDetails { get; }

		public CancellationToken Token { get; }
	}
}