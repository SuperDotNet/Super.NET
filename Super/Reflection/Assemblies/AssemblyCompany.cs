using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyCompany : Attribute<AssemblyCompanyAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyCompany();

		AssemblyCompany() : base(x => x.Company) {}
	}
}