using Super.Model.Sequences;
using System.Reflection;

namespace Super.Reflection
{
	public interface IAttributes<T> : IArray<ICustomAttributeProvider, T> {}
}