using System.Reflection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Reflection
{
	public interface IMetadata<T> : IConditional<ICustomAttributeProvider, Array<T>> {}
}