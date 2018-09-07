using Super.Model.Collections;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	public sealed class Assemblies : Sequence<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName)) {}
	}
}