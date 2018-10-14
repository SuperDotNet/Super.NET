using System.Collections.Generic;

namespace Super.Model.Collections.Groups
{
	public interface IGroup<T> : IList<T>
	{
		GroupName Name { get; }
	}
}