using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Model.Selection.Conditions;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Model.Results
{
	public interface IMutable<T> : IResult<T>, ICommand<T> {}

	public class Variable<T> : IMutable<T>
	{
		T _instance;

		public Variable(T instance = default) => _instance = instance;

		public T Get() => _instance;

		public void Execute(T parameter)
		{
			_instance = parameter;
		}
	}

	public class DecoratedMutable<T> : IMutable<T>
	{
		readonly IMutable<T> _mutable;

		public DecoratedMutable(IMutable<T> mutable) => _mutable = mutable;

		public T Get() => _mutable.Get();

		public void Execute(T parameter)
		{
			_mutable.Execute(parameter);
		}
	}

	public interface IStore<T> : IMutable<T>, IConditionAware<None> {}

	public class Store<T> : DecoratedMutable<T>, IStore<T>, IActivateUsing<IMutable<T>>
	{
		[UsedImplicitly]
		public Store(IMutable<T> mutable) : this(mutable.Select(IsAssigned<T>.Default).ToSelect().ToCondition(),
		                                         mutable) {}

		public Store(ICondition<None> condition, IMutable<T> mutable) : base(mutable) => Condition = condition;

		public ICondition<None> Condition { get; }
	}
}