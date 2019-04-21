using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using System.Reflection;

namespace Super.Reflection
{
	public interface IMetadata<T> : IConditional<ICustomAttributeProvider, Array<T>> {}
}