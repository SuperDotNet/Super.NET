using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public sealed class GenericInterfaces : ISelect<TypeInfo, IEnumerable<TypeInfo>>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() {}

		public IEnumerable<TypeInfo> Get(TypeInfo parameter)
			=> AllInterfaces.Default.Get(parameter)
			                .ToArray()
			                .Prepend(parameter)
			                .Where(x => x.IsGenericType)
			                .Distinct();
	}
}