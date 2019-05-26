using System;
using Super.Model.Selection.Alterations;
using Super.Runtime.Invocation;

namespace Super.Model.Selection.Adapters
{
	public class ResultDelegateSelector<_, T> : Selector<_, Func<T>>
	{
		public ResultDelegateSelector(ISelect<_, Func<T>> subject) : base(subject) {}

		public Selector<_, Func<T>> Singleton() => Select(SingletonDelegate<T>.Default);

		public Selector<_, T> Invoke() => Select(Call<T>.Default);
	}
}