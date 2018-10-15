using Super.Model.Selection.Stores;
using Super.Reflection;
using System;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class AssemblyLocation : ReferenceStore<Assembly, Uri>
	{
		public static AssemblyLocation Default { get; } = new AssemblyLocation();

		AssemblyLocation() : base(Start.From<Assembly>().Select(x => x.CodeBase).Select(I<Uri>.Default.New).Get) {}
	}
}