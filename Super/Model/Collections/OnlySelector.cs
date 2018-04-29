using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class OnlySelector<T> : ISelect<IEnumerable<T>, T>
	{
		public static OnlySelector<T> Default { get; } = new OnlySelector<T>();

		OnlySelector() : this(x => true) {}

		readonly Func<T, bool> _where;

		public OnlySelector(Func<T, bool> where) => _where = where;

		public T Get(IEnumerable<T> parameter)
		{
			var enumerable = parameter.Where(_where).ToArray();
			var result     = enumerable.Length == 1 ? enumerable[0] : default;
			return result;
		}
	}

	public interface ISequenceAlteration<T> : IAlteration<IEnumerable<T>> {}

	public class WhereSelector<T> : ISequenceAlteration<T>
	{
		readonly Func<T, bool> _where;

		public WhereSelector(Func<T, bool> where) => _where = where;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Where(_where);
	}
}