using System.Collections.Generic;
using Super.ExtensionMethods;
using Super.Model.Sources.Alterations;

namespace Super.Model.Sources
{
	sealed class CompositeAlteration<T> : IAlteration<T>
	{
		readonly IEnumerable<IAlteration<T>> _alterations;

		public CompositeAlteration(IEnumerable<IAlteration<T>> alterations) => _alterations = alterations;

		public T Get(T parameter) => _alterations.Alter(parameter);
	}
}