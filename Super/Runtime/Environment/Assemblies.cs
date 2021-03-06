﻿using System.Reflection;
using Super.Compose;
using Super.Model.Sequences;
using Super.Reflection.Assemblies;

namespace Super.Runtime.Environment
{
	sealed class Assemblies : ArrayResult<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(A.This(PrimaryAssembly.Default)
		                     .Select(AssemblyNameSelector.Default)
		                     .Select(ComponentAssemblyNames.Default)
		                     .Query()
		                     .Select(Load.Default)
		                     .Append(HostingAssembly.Default.And(PrimaryAssembly.Default))
		                     .WhereBy(y => y != null)
		                     .Distinct()
		                     .Get()
		                     .ToResult()) {}
	}
}