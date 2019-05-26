using System;
using Super.Model.Results;
using Super.Model.Selection.Conditions;
using Super.Runtime.Invocation;

namespace Super.Runtime.Environment
{
	public class SystemStore<T> : Deferred<T>, IStore<T>
	{
		readonly IStore<T> _store;

		protected SystemStore(Func<T> source) : this(source.Start()) {}

		protected SystemStore(IResult<T> result) : this(result, SystemStores.New<T>()) {}

		protected SystemStore(IResult<T> result, IStore<T> store) : this(store.Condition, result, store) {}

		protected SystemStore(ICondition<None> condition, IResult<T> result, IStore<T> store) : base(result, store)
		{
			Condition = condition;
			_store    = store;
		}

		public void Execute(T parameter)
		{
			_store.Execute(parameter);
		}

		public ICondition<None> Condition { get; }
	}
}