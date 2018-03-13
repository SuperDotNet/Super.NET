using System.Collections.Generic;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public interface IGroupCollection<T> : IEnumerable<T>, ISource<GroupName, IList<T>> {}
}