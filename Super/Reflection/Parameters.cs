using Super.Model.Sources;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection
{
	sealed class Parameters : DelegatedSource<ConstructorInfo, ICollection<ParameterInfo>>
	{
		public static Parameters Default { get; } = new Parameters();

		Parameters() : base(x => x.GetParameters()) {}
	}
}