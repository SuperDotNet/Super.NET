using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblyNames : ISource<AssemblyName, IEnumerable<AssemblyName>>
	{
		public static ComponentAssemblyNames Default { get; } = new ComponentAssemblyNames();

		ComponentAssemblyNames() : this(EnvironmentAssemblyName.Default, PlatformAssemblyName.Default) {}

		readonly Func<AssemblyName, IEnumerable<AssemblyName>> _expand;
		readonly ImmutableArray<IAlteration<AssemblyName>> _names;

		public ComponentAssemblyNames(params IAlteration<AssemblyName>[] names)
			: this(ComponentAssemblyCandidates.Default.ToDelegate(), names.ToImmutableArray()) {}

		public ComponentAssemblyNames(Func<AssemblyName, IEnumerable<AssemblyName>> expand, ImmutableArray<IAlteration<AssemblyName>> names)
		{
			_expand = expand;
			_names = names;
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