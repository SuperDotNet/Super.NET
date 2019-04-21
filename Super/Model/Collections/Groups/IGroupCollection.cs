using Super.Model.Selection;
using Super.Model.Sequences;
using System.Collections.Generic;

namespace Super.Model.Collections.Groups
{
	public interface IGroupCollection<T> : ISelect<GroupName, IList<T>>
	{
		IArray<T> Values { get; }
	}
}