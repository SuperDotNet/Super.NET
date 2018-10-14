using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public sealed class AppendDelegatedValue<T> : IAlteration<IEnumerable<T>>
	{
		readonly ISource<T> _item;

		public AppendDelegatedValue(ISource<T> item) => _item = item;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Append(_item.Get());
	}
}