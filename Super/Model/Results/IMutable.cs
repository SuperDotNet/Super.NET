using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Model.Selection.Conditions;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

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

	public class Mutable<T> : IMutable<T>
	{
		readonly Action<T> _set;
		readonly Func<T>   _get;

		public Mutable(IMutable<T> mutable) : this(mutable.Execute, mutable.Get) {}

		public Mutable(Action<T> set, Func<T> get)
		{
			_set = set;
			_get = get;
		}

		public T Get() => _get();

		public void Execute(T parameter)
		{
			_set(parameter);
		}
	}

	public interface IStore<T> : IMutable<T>, IConditionAware<None> {}

	public class Store<T> : Mutable<T>, IStore<T>, IActivateUsing<IMutable<T>>
	{
		[UsedImplicitly]
		public Store(IMutable<T> mutable) : this(mutable.Select(IsAssigned<T>.Default).ToSelect().ToCondition(),
		                                         mutable) {}

		public Store(ICondition<None> condition, IMutable<T> mutable) : base(mutable) => Condition = condition;

		public ICondition<None> Condition { get; }
	}
}