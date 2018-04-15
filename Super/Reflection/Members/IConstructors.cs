using System.Collections.Generic;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Members
{
	interface IConstructors : ISelect<TypeInfo, ICollection<ConstructorInfo>> {}
}