using System.Collections.Generic;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Members
{
	sealed class Parameters : Select<ConstructorInfo, ICollection<ParameterInfo>>
	{
		public static Parameters Default { get; } = new Parameters();

		Parameters() : base(x => x.GetParameters()) {}
	}
}