using System.Collections.Generic;

namespace Super.Model.Sequences.Collections.Groups
{
	public interface IGroup<T> : IList<T>
	{
		GroupName Name { get; }
	}
}