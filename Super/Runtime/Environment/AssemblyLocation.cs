using Super.Compose;
using Super.Model.Selection.Stores;
using Super.Reflection;
using System;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class AssemblyLocation : ReferenceValueStore<Assembly, Uri>
	{
		public static AssemblyLocation Default { get; } = new AssemblyLocation();

		AssemblyLocation() : base(Start.A.Selection<Assembly>()
		                               .By.Calling(x => x.CodeBase)
		                               .Select(I.A<Uri>().New)
		                               .Get) {}
	}
}