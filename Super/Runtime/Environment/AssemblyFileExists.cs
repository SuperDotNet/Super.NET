using System.Reflection;
using Super.Io;
using Super.Model.Selection.Alterations;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	sealed class AssemblyFileExists : DecoratedSpecification<Assembly>, IActivateMarker<IAlteration<string>>
	{
		public AssemblyFileExists(IAlteration<string> alter)
			: base(AssemblyLocation.Default.Select(LocalFilePath.Default).Select(alter).Out(FilePathExists.Default)) {}
	}
}