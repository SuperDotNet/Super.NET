using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public interface ITyped<out T> : ISource<TypeInfo, T> {}
}