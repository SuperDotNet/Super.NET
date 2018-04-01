using Super.ExtensionMethods;
using Super.Model.Sources;
using System.IO;
using System.Reflection;

namespace Super.Reflection
{
	sealed class Load : DecoratedSource<AssemblyName, Assembly>
	{
		public static Load Default { get; } = new Load();

		Load() : base(new DelegatedSource<AssemblyName, Assembly>(Assembly.Load).Try(I<FileNotFoundException>.Default)) {}
	}
}