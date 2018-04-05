using Super.Model.Sources;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection
{
	interface IConstructors : ISource<TypeInfo, ICollection<ConstructorInfo>> {}

	sealed class Constructors : DelegatedSource<TypeInfo, ICollection<ConstructorInfo>>, IConstructors
	{
		public static Constructors Default { get; } = new Constructors();
		Constructors() : base(x => x.GetConstructors()) {}
	}
}
