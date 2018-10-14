using System;
using Super.Runtime.Activation;

namespace Super.Model.Sequences.Query {
	public class One<T> : IReduce<T>, IActivateMarker<Func<T, bool>>
	{
		readonly Func<Array<T>, Array<T>> _where;
		readonly Func<T>                  _default;

		public One(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public One(Func<T, bool> where, Func<T> @default) : this(new Where<T>(where).Get, @default) {}

		public One(Func<Array<T>, Array<T>> where, Func<T> @default)
		{
			_where   = @where;
			_default = @default;
		}

		public T Get(Array<T> parameter)
		{
			var enumerable = _where(parameter);
			var result     = enumerable.Length == 1 ? enumerable[0] : _default();
			return result;
		}
	}
}