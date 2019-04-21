using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyProduct : Declared<AssemblyProductAttribute, string>
	{
		public static AssemblyProduct Default { get; } = new AssemblyProduct();

		AssemblyProduct() : base(x => x.Product) {}
	}
}