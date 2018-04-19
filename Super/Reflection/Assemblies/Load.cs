using Super.Model.Selection;
using System.IO;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class Load : DecoratedSelect<AssemblyName, Assembly>
	{
		public static Load Default { get; } = new Load();

		Load() : base(In<AssemblyName>.Select(Assembly.Load).Try(I<FileNotFoundException>.Default)) {}
	}
}