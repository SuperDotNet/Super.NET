using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Members
{
	public sealed class PropertyAccessMethodSelector : ISelect<PropertyInfo, MethodInfo>
	{
		public static PropertyAccessMethodSelector Default { get; } = new PropertyAccessMethodSelector();

		PropertyAccessMethodSelector() {}

		public MethodInfo Get(PropertyInfo parameter) => parameter.GetMethod;
	}
}