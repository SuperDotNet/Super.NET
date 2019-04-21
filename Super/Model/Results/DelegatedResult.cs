using Super.Runtime.Activation;
using System;

namespace Super.Model.Results
{
	public class DelegatedResult<T> : IResult<T>, IActivateUsing<Func<T>>
	{
		public static implicit operator T(DelegatedResult<T> result) => result.Get();

		readonly Func<T> _source;

		public DelegatedResult(Func<T> source) => _source = source;

		public T Get() => _source();
	}

	public class WrappedResult<T> : IResult<T>
	{
		public static implicit operator T(WrappedResult<T> result) => result.Get();

		readonly Func<Func<T>> _result;

		public WrappedResult(Func<Func<T>> result) => _result = result;

		public T Get() => _result()();
	}
}