using System.Collections.Generic;

namespace Super.Model.Collections
{
	public interface IElements<T> : IMembership<T>, IEnumerable<T> {}
}