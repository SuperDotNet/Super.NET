using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public interface ITyped<out T> : ISelect<TypeInfo, T> {}
}