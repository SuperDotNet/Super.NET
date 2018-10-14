using System.Collections.Generic;
using System.Linq;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Model.Collections {
	public class Has<T> : DelegatedSpecification<T>, IActivateMarker<ICollection<T>>, IActivateMarker<IEnumerable<T>>
	{
		public Has(ICollection<T> source) : base(source.Contains) {}

		public Has(IEnumerable<T> source) : base(source.Contains) {}
	}
}