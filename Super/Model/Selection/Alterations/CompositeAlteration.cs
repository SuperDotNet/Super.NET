using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Selection.Alterations
{
	public class CompositeAlteration<T> : IAlteration<T>
	{
		readonly ImmutableArray<IAlteration<T>> _alterations;

		public CompositeAlteration(params IAlteration<T>[] alterations) : this(alterations.ToImmutableArray()) {}

		public CompositeAlteration(ImmutableArray<IAlteration<T>> alterations) => _alterations = alterations;

		public T Get(T parameter) => _alterations.ToArray().Alter(parameter);
	}
}