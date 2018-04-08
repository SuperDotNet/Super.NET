using System.Collections.Generic;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	interface IConstructors : ISelect<TypeInfo, ICollection<ConstructorInfo>> {}

	sealed class Constructors : Delegated<TypeInfo, ICollection<ConstructorInfo>>, IConstructors
	{
		public static Constructors Default { get; } = new Constructors();
		Constructors() : base(x => x.GetConstructors()) {}
	}
}
