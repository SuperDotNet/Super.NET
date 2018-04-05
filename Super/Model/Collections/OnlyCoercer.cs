using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class OnlyCoercer<T> : ISource<IEnumerable<T>, T>
	{
		public static OnlyCoercer<T> Default { get; } = new OnlyCoercer<T>();

		OnlyCoercer() : this(Where<T>.Always) {}

		readonly Func<T, bool> _where;

		public OnlyCoercer(Func<T, bool> where) => _where = where;

		public T Get(IEnumerable<T> parameter)
		{
			var enumerable = parameter.Where(_where).ToArray();
			var result     = enumerable.Length == 1 ? enumerable[0] : default;
			return result;
		}
	}

	public sealed class WhereSelector<T> : IAlteration<IEnumerable<T>>
	{
		readonly Func<T, bool> _where;

		public WhereSelector(Func<T, bool> where) => _where = where;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Where(_where);
	}
}