using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyProduct : Attribute<AssemblyProductAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyProduct();

		AssemblyProduct() : base(x => x.Product) {}
	}
}