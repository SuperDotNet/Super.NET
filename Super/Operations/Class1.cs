using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using System;
using System.Threading.Tasks;

namespace Super.Operations
{
	public static class Extensions {}

	public class OperationSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public OperationSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Demote() => new TaskSelector<_, T>(Get().Select(SelectTask<T>.Default));

		public TaskSelector<_, TTo> Then<TTo>(ISelect<T, TTo> select) => Then(select.Get);

		public TaskSelector<_, TTo> Then<TTo>(Func<T, TTo> select) => Demote().Then(select);
	}

	public class TaskSelector<_, T> : Selector<_, Task<T>>
	{
		public TaskSelector(ISelect<_, Task<T>> subject) : base(subject) {}

		public OperationSelector<_, T> Promote()
			=> new OperationSelector<_, T>(Get().Select(SelectOperation<T>.Default));

		public TaskSelector<_, TTo> Then<TTo>(ISelect<T, TTo> select) => Then(select.Get);

		public TaskSelector<_, TTo> Then<TTo>(Func<T, TTo> select)
			=> new TaskSelector<_, TTo>(Get().Select(new Selection<T, TTo>(select)));
	}

	public class Selection<TIn, TOut> : ISelect<Task<TIn>, Task<TOut>>
	{
		readonly Func<Task<TIn>, TOut> _select;

		public Selection(Func<TIn, TOut> select) : this(new Handle<TIn, TOut>(select).Get) {}

		public Selection(Func<Task<TIn>, TOut> select) => _select = select;

		public Task<TOut> Get(Task<TIn> parameter) => parameter.ContinueWith(_select);
	}

	public sealed class Handle<TIn, TOut> : ISelect<Task<TIn>, TOut>
	{
		readonly Func<TIn, TOut> _select;

		public Handle(Func<TIn, TOut> select) => _select = select;

		public TOut Get(Task<TIn> parameter) => _select(parameter.Result);
	}

	public sealed class SelectOperation<T> : Select<Task<T>, ValueTask<T>>
	{
		public static SelectOperation<T> Default { get; } = new SelectOperation<T>();

		SelectOperation() : base(x => new ValueTask<T>(x)) {}
	}

	public sealed class SelectTask<T> : Select<ValueTask<T>, Task<T>>
	{
		public static SelectTask<T> Default { get; } = new SelectTask<T>();

		SelectTask() : base(x => x.AsTask()) {}
	}

	public interface IOperation<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>> {}

	public class Operation<TIn, TOut> : Select<TIn, ValueTask<TOut>>, IOperation<TIn, TOut>
	{
		public Operation(Func<TIn, ValueTask<TOut>> select) : base(select) {}
	}

	public interface IOperation<T> : IResult<ValueTask<T>> {}

	class Operation<T> : Instance<ValueTask<T>>, IOperation<T>
	{
		public Operation(Task<T> instance) : this(new ValueTask<T>(instance)) {}

		public Operation(T instance) : this(new ValueTask<T>(instance)) {}

		public Operation(ValueTask<T> instance) : base(instance) {}
	}

	class Class1 {}
}