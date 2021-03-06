using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblyNames : ISelect<AssemblyName, IEnumerable<AssemblyName>>
	{
		public static ComponentAssemblyNames Default { get; } = new ComponentAssemblyNames();

		ComponentAssemblyNames() : this(EnvironmentAssemblyName.Default) {}

		readonly Func<AssemblyName, IEnumerable<AssemblyName>> _expand;
		readonly ImmutableArray<IAlteration<AssemblyName>>     _names;

		public ComponentAssemblyNames(params IAlteration<AssemblyName>[] names)
			: this(ComponentAssemblyCandidates.Default.Get, names.ToImmutableArray()) {}

		public ComponentAssemblyNames(Func<AssemblyName, IEnumerable<AssemblyName>> expand,
		                              ImmutableArray<IAlteration<AssemblyName>> names)
		{
			_expand = expand;
			_names  = names;
		}

		public IEnumerable<AssemblyName> Get(AssemblyName parameter)
		{
			foreach (var name in _expand(parameter))
			{
				foreach (var alteration in _names)
				{
					yield return alteration.Get(name);
				}
			}
		}
	}
}