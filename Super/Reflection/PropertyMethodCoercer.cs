using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public sealed class PropertyMethodCoercer : ISource<PropertyInfo, MethodInfo>
	{
		public static PropertyMethodCoercer Default { get; } = new PropertyMethodCoercer();

		PropertyMethodCoercer() {}

		public MethodInfo Get(PropertyInfo parameter) => parameter.GetMethod;
	}
}