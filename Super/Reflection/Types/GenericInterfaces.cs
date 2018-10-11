using Super.Model.Collections;
using Super.Model.Selection.Stores;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericInterfaces : Store<TypeInfo, Array<TypeInfo>>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : base(AllInterfaces.Default.Select(x => x.Where(y => y.IsGenericType)).Result().Get) {}
	}
}