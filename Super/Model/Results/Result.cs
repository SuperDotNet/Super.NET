using Super.Runtime.Activation;
using System;

namespace Super.Model.Results
{
	public class Result<T> : IResult<T>, IActivateUsing<Func<T>>, IActivateUsing<IResult<T>>
	{
		public static implicit operator T(Result<T> result) => result.Get();

		readonly Func<T> _source;

		public Result(IResult<T> result) : this(result.Get) {}

		public Result(Func<T> source) => _source = source;

		public T Get() => _source();
	}

	public class Assume<T> : IResult<T>
	{
		public static implicit operator T(Assume<T> result) => result.Get();

		readonly Func<Func<T>> _result;

		public Assume(Func<Func<T>> result) => _result = result;

		public T Get() => _result()();
	}
}