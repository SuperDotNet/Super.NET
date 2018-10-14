using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class AllInterfaces : Store<TypeInfo, Array<TypeInfo>>
	{
		public static AllInterfaces Default { get; } = new AllInterfaces();

		AllInterfaces() : base(Interfaces.Default.Select(x => x.Where(y => y.IsInterface).Distinct()).Result().Get) {}
	}
}