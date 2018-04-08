using Super.Model.Selection;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class AssemblyNameSelector : ISelect<Assembly, AssemblyName>
	{
		public static AssemblyNameSelector Default { get; } = new AssemblyNameSelector();

		AssemblyNameSelector() {}

		public AssemblyName Get(Assembly parameter) => parameter.GetName();
	}
}