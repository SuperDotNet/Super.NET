using System.Reflection;
using Super.Io;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	sealed class AssemblyFileExists : Condition<Assembly>, IActivateUsing<IAlteration<string>>
	{
		public AssemblyFileExists(IAlteration<string> alter)
			: base(AssemblyLocation.Default.Select(LocalFilePath.Default)
			                       .Select(alter)
			                       .Select(FilePathExists.Default)
			                       .Then()) {}
	}
}