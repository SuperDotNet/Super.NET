using Super.Model.Sources;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public interface IItems<out T> : ISource<IEnumerable<T>> {}
}