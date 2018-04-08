using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public interface IGroupCollection<T> : IEnumerable<T>, ISelect<GroupName, IList<T>> {}
}