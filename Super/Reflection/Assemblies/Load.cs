using Super.Compose;
using Super.Model.Selection;
using System.IO;
using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class Load : Select<AssemblyName, Assembly>
	{
		public static Load Default { get; } = new Load();

		Load() : base(Start.A.Selection<AssemblyName>()
		                   .By.Calling(Assembly.Load)
		                   .Then()
		                   .Try<FileNotFoundException>()
		                   .Get()) {}
	}
}