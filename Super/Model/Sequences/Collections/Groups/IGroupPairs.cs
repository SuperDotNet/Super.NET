using System.Collections.Generic;
using Super.Model.Selection;
using Super.Runtime;

namespace Super.Model.Sequences.Collections.Groups
{
	public interface IGroupPairs<T> : ISelect<IGroup<T>, Pair<GroupName, IList<T>>> {}
}