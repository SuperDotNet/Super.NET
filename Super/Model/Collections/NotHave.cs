using System.Collections.Generic;
using Super.Model.Specifications;
using Super.Runtime.Activation;

namespace Super.Model.Collections {
	public class NotHave<T> : InverseSpecification<T>, IActivateMarker<ICollection<T>>, IActivateMarker<IEnumerable<T>>
	{
		public NotHave(ICollection<T> source) : base(new Has<T>(source)) {}

		public NotHave(IEnumerable<T> source) : base(new Has<T>(source)) {}
	}
}