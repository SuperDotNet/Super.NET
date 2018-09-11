using System;

namespace Super.Reflection
{
	public interface IDeclared<T> : IAttribute<ReadOnlyMemory<T>> {}
}