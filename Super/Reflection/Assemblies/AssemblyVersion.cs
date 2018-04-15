using System;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyVersion : Attribute<AssemblyFileVersionAttribute, Version>
	{
		public static IAttribute<Version> Default { get; } = new AssemblyVersion();

		AssemblyVersion() : base(x => Version.Parse(x.Version)) {}
	}
}