using Super.ExtensionMethods;
using System.IO;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	sealed class Load : Decorated<AssemblyName, Assembly>
	{
		public static Load Default { get; } = new Load();

		Load() : base(In<AssemblyName>.Select(Assembly.Load).Try(I<FileNotFoundException>.Default)) {}
	}
}