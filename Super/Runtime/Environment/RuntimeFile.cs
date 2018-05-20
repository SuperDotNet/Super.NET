using System.IO;
using Super.Model.Selection.Alterations;
using Super.Runtime.Invocation;

namespace Super.Runtime.Environment
{
	class RuntimeFile : Invocation0<string, string, string>, IAlteration<string>
	{
		protected RuntimeFile(string extension) : base(Path.ChangeExtension, extension) {}
	}
}