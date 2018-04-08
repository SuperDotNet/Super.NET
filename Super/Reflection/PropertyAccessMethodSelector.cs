using Super.Model.Selection;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class PropertyAccessMethodSelector : ISelect<PropertyInfo, MethodInfo>
	{
		public static PropertyAccessMethodSelector Default { get; } = new PropertyAccessMethodSelector();

		PropertyAccessMethodSelector() {}

		public MethodInfo Get(PropertyInfo parameter) => parameter.GetMethod;
	}
}