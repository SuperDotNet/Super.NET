using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public interface IAttribute<out T> : ISelect<ICustomAttributeProvider, T> {}
}