using Super.Compose;
using System;

namespace Super.Model.Sequences.Query
{
	public class One<T> : IElement<T>
	{
		readonly Func<ArrayView<T>, ArrayView<T>> _where;
		readonly Func<T>                          _default;

		public One(Func<T, bool> where) : this(where, A.Default<T>) {}

		public One(Func<T, bool> where, Func<T> @default) : this(new Where<T>(where).Get, @default) {}

		public One(Func<ArrayView<T>, ArrayView<T>> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(ArrayView<T> parameter)
		{
			var view   = _where(parameter);
			var result = view.Length == 1 ? view.Array[0] : _default();
			return result;
		}
	}
}