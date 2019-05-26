using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Sequences.Collections.Groups
{
	public interface IGroupCollection<T> : ISelect<GroupName, IList<T>>
	{
		IArray<T> Values { get; }
	}
}