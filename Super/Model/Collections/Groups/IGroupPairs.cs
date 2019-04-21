using Super.Model.Selection;
using Super.Runtime;
using System.Collections.Generic;

namespace Super.Model.Collections.Groups
{
	public interface IGroupPairs<T> : ISelect<IGroup<T>, Pair<GroupName, IList<T>>> {}
}