using System;
using System.Linq;
using System.Reflection;
using Super.Model.Sequences;

namespace Super.Reflection.Assemblies
{
	public sealed class Assemblies : ArrayInstance<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName)) {}
	}
}