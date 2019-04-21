using Super.Model.Results;
using Super.Model.Selection.Stores;
using Super.Runtime;
using System;

namespace Super.Model.Selection.Adapters
{
	public class Result<T> : DelegatedResult<T>, ISelect<T>
	{
		public static implicit operator Result<T>(Func<T> value) => new Result<T>(value);

		public static implicit operator Func<T>(Result<T> value) => value.Get;

		readonly Func<T> _source;

		public Result(Func<T> source) : base(source) => _source = source;

		public T Get(None _) => _source();
	}

	sealed class Delegates<T> : ReferenceValueStore<ISelect<None, T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}
