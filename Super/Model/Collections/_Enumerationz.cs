using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class OnlySelector<T> : ISelect<IEnumerable<T>, T>
	{
		public static OnlySelector<T> Default { get; } = new OnlySelector<T>();

		OnlySelector() : this(Always<T>.Default.IsSatisfiedBy) {}

		readonly Func<T, bool> _where;

		public OnlySelector(Func<T, bool> where) => _where = where;

		public T Get(IEnumerable<T> parameter)
		{
			var enumerable = parameter.Where(_where).ToArray();
			var result     = enumerable.Length == 1 ? enumerable[0] : default;
			return result;
		}
	}

	public sealed class Only<T> : ISelect<ReadOnlyMemory<T>, T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.IsSatisfiedBy) {}

		readonly Func<ReadOnlyMemory<T>, ReadOnlyMemory<T>> _where;

		public Only(Func<T, bool> where) : this(new Where<T>(where).Get) {}

		public Only(Func<ReadOnlyMemory<T>, ReadOnlyMemory<T>> where) => _where = where;

		public T Get(ReadOnlyMemory<T> parameter)
		{
			var enumerable = _where(parameter);
			var result     = enumerable.Length == 1 ? enumerable.Span[0] : default;
			return result;
		}
	}

	/*public interface ISequenceAlteration<T> : IAlteration<IEnumerable<T>> {}*/

	public class WhereSelector<T> : IEnumerableAlteration<T>
	{
		readonly Func<T, bool> _where;

		public WhereSelector(Func<T, bool> where) => _where = where;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Where(_where);
	}

	public interface IEnumerableAlteration<T> : IAlteration<IEnumerable<T>> {}

	public class EnumerableAlterations<T> : IEnumerableAlteration<T>
	{
		readonly ReadOnlyMemory<IAlteration<IEnumerable<T>>> _alterations;

		public EnumerableAlterations(params IAlteration<IEnumerable<T>>[] alterations) => _alterations = alterations;

		public IEnumerable<T> Get(IEnumerable<T> parameter)
			=> _alterations.AsEnumerable()
			               .Aggregate(parameter, (enumerable, alteration) => alteration.Get(enumerable));
	}
}
