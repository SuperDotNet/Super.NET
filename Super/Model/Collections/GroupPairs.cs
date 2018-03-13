using System.Collections.Generic;
using Super.Runtime;

namespace Super.Model.Collections
{
	sealed class GroupPairs<T> : IGroupPairs<T>
	{
		public static GroupPairs<T> Default { get; } = new GroupPairs<T>();

		GroupPairs() {}

		public KeyValuePair<GroupName, IList<T>> Get(IGroup<T> parameter)
			=> Pairs.Create(parameter.Name, (IList<T>)parameter);
	}
}