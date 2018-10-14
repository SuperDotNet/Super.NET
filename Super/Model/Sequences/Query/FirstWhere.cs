using System;
using Super.Model.Specifications;

namespace Super.Model.Sequences.Query {
	public class FirstWhere<T> : IReduce<T>
	{
		readonly Func<T, bool> _where;
		readonly Func<T>       _default;

		public FirstWhere(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public FirstWhere(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = @where;
			_default = @default;
		}

		public T Get(Array<T> parameter)
		{
			var length = parameter.Length;

			for (var i = 0; i < length; i++)
			{
				var item = parameter[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default.Invoke();
		}
	}
}