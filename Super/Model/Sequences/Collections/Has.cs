using System.Collections.Generic;
using System.Linq;
using Super.Model.Selection.Conditions;
using Super.Runtime.Activation;

namespace Super.Model.Sequences.Collections
{
	public class Has<T> : Condition<T>, IActivateUsing<ICollection<T>>, IActivateUsing<IEnumerable<T>>
	{
		public Has(ICollection<T> source) : base(source.Contains) {}

		public Has(IEnumerable<T> source) : base(source.Contains) {}
	}
}