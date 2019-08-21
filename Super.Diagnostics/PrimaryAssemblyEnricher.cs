using Humanizer;
using Serilog.Core.Enrichers;
using Super.Reflection.Assemblies;
using Super.Runtime.Environment;

namespace Super.Diagnostics.Logging
{
	sealed class PrimaryAssemblyEnricher : PropertyEnricher
	{
		public static PrimaryAssemblyEnricher Default { get; } = new PrimaryAssemblyEnricher();

		PrimaryAssemblyEnricher() : this(PrimaryAssemblyDetails.Default.Get()) {}

		public PrimaryAssemblyEnricher(AssemblyDetails value)
			: base(nameof(PrimaryAssembly).Humanize(), value, true) {}
	}
}
