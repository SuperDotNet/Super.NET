using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public interface IGroupPairs<T> : ISelect<IGroup<T>, KeyValuePair<GroupName, IList<T>>> {}
}