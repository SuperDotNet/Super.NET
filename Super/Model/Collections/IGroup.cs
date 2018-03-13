using System.Collections.Generic;

namespace Super.Model.Collections
{
	public interface IGroup<T> : IList<T>
	{
		GroupName Name { get; }
	}
}