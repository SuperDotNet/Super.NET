using Super.Model.Collections;
using System.Reflection;

namespace Super.Reflection
{
	public interface IAttributes<T> : IArray<ICustomAttributeProvider, T> {}
}