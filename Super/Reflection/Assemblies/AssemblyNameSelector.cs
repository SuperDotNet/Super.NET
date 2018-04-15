﻿using Super.Model.Selection;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	public sealed class AssemblyNameSelector : Delegated<Assembly, AssemblyName>
	{
		public static AssemblyNameSelector Default { get; } = new AssemblyNameSelector();

		AssemblyNameSelector() : base(x => x.GetName()) {}
	}
}