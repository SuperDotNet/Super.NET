using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyCompany : Declared<AssemblyCompanyAttribute, string>
	{
		public static AssemblyCompany Default { get; } = new AssemblyCompany();

		AssemblyCompany() : base(x => x.Company) {}
	}
}