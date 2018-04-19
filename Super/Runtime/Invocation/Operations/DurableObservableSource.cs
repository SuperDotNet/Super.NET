using Polly;
using Super.Model.Selection;
using Super.Runtime.Execution;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Runtime.Invocation.Operations
{
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

	class Operation<T> : ISelect<T, Task>
	{
		readonly Func<T, Task> _task;
		readonly Func<string, IDisposable> _context;
		readonly Func<OperationState> _state;

		public Operation(Func<T, Task> task, Func<string, IDisposable> context)
		{
			_task = task;
			_context = context;
			
		}

		public Task Get(T parameter)
		{
			/*var context = _context(string.Empty);
			/*var state = new Logical<OperationState>(new OperationState(""));#1#
			var result = _task(parameter).ContinueWith()
			return result;*/
			return null;
		}
	}

	sealed class OperationState
	{
		public OperationState(string name, CancellationToken token = new CancellationToken())
			: this(new Execution.Context(name), token) {}

		public OperationState(Execution.Context context, CancellationToken token = new CancellationToken())
		{
			Context = context;
			Token   = token;
		}

		public Execution.Context Context { get; }

		public CancellationToken Token { get; }
	}
}