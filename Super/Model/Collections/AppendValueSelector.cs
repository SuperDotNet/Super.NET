using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class AppendValueSelector<T> : IAlteration<IEnumerable<T>>
	{
		readonly ISource<T> _item;

		public AppendValueSelector(ISource<T> item) => _item = item;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Append(_item.Get());
	}

	public sealed class AppendSelector<T> : IAlteration<IEnumerable<T>>
	{
		readonly T[] _item;

		public AppendSelector(params T[] items) => _item = items.ToArray();

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Append(_item);
	}
}