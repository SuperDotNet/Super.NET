using Super.Model.Selection.Alterations;

namespace Super.Runtime.Environment
{
	sealed class DevelopmentRuntimeFile : RuntimeFile
	{
		public static IAlteration<string> Default { get; } = new DevelopmentRuntimeFile();

		DevelopmentRuntimeFile() : base(".runtimeconfig.dev.json") {}
	}
}