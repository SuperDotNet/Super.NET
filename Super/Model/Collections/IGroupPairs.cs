using System.Collections.Generic;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public interface IGroupPairs<T> : ISource<IGroup<T>, KeyValuePair<GroupName, IList<T>>> {}
}