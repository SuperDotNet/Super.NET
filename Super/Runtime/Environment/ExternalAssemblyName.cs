using Super.Model.Sources.Alterations;
using System.Reflection;

namespace Super.Runtime.Environment
{
	class ExternalAssemblyName : IAlteration<AssemblyName>
	{
		readonly string _format;

		public ExternalAssemblyName(string format) => _format = format;

		public AssemblyName Get(AssemblyName parameter) => new AssemblyName(string.Format(_format, parameter.Name));
	}
}