using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Model.Sources.Alterations;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public sealed class AppendInstanceCoercer<T> : IAlteration<IEnumerable<T>>
	{
		readonly IInstance<T> _item;

		public AppendInstanceCoercer(IInstance<T> item) => _item = item;

		public IEnumerable<T> Get(IEnumerable<T> parameter) => parameter.Append(_item.Get());
	}
}