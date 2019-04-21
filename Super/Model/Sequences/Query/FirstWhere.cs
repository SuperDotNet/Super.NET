using Super.Model.Results;
using Super.Model.Selection.Conditions;
using System;

namespace Super.Model.Sequences.Query
{
	public class FirstWhere<T> : IElement<T>
	{
		readonly Func<T, bool> _where;
		readonly Func<T>       _default;

		public FirstWhere(ICondition<T> where) : this(where.Get) {}

		public FirstWhere(Func<T, bool> where) : this(@where, Default<T>.Instance.Get) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = @where;
			_default = @default;
		}

		public T Get(ArrayView<T> parameter)
		{
			var length = parameter.Length;

			for (var i = parameter.Start; i < length; i++)
			{
				var item = parameter.Array[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default.Invoke();
		}
	}
}