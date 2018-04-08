using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Super.Model.Selection.Alterations;

namespace Super.Model.Collections
{
	public class EnumerableAlterations<T> : IEnumerableAlteration<T>
	{
		readonly ImmutableArray<IAlteration<IEnumerable<T>>> _alterations;

		public EnumerableAlterations(params IAlteration<IEnumerable<T>>[] alterations) :
			this(alterations.ToImmutableArray()) {}

		public EnumerableAlterations(ImmutableArray<IAlteration<IEnumerable<T>>> alterations) => _alterations = alterations;

		public IEnumerable<T> Get(IEnumerable<T> parameter)
			=> _alterations.Aggregate(parameter, (enumerable, alteration) => alteration.Get(enumerable));
	}
}