using System;
using System.Reactive;

namespace Super.Model.Selection
{
	class DelegatedResult<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TResult> _result;

		public DelegatedResult(Func<TResult> source) => _result = source;

		public TResult Get(TParameter _) => _result();
	}

	class Any<T> : IAny<T>
	{
		readonly ISelect<Unit, T> _select;

		public Any(Func<T> source) : this(new DelegatedResult<Unit, T>(source)) {}

		public Any(ISelect<Unit, T> select) => _select = @select;

		public T Get(Unit parameter) => _select.Get(Unit.Default);

		public T Get(object parameter) => Get(Unit.Default);
	}

	public interface IAny<out T> : ISelect<object, T>, ISelect<Unit, T> {}
}