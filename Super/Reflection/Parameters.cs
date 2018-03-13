using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class Parameters : ISource<ConstructorInfo, IEnumerable<ParameterInfo>>
	{
		public static Parameters Default { get; } = new Parameters();

		Parameters() {}

		public IEnumerable<ParameterInfo> Get(ConstructorInfo parameter) => parameter.GetParameters().Hide();
	}
}