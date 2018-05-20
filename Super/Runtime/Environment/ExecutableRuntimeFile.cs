using Super.Model.Selection.Alterations;

namespace Super.Runtime.Environment
{
	sealed class ExecutableRuntimeFile : RuntimeFile
	{
		public static IAlteration<string> Default { get; } = new ExecutableRuntimeFile();

		ExecutableRuntimeFile() : base(".exe") {}
	}
}