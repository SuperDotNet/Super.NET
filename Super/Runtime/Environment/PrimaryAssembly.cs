﻿using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Reflection;

namespace Super.Runtime.Environment
{
	sealed class PrimaryAssembly : Instance<Assembly>
	{
		public static PrimaryAssembly Default { get; } = new PrimaryAssembly();

		PrimaryAssembly() : base(Assemblies.Default.Only(x => x.Has<PrimaryAssemblyAttribute>())) {}
	}
}