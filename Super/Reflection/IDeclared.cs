using System.Collections.Generic;

namespace Super.Reflection
{
	public interface IDeclared<out T> : IAttribute<IEnumerable<T>> {}
}