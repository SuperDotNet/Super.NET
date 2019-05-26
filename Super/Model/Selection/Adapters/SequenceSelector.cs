using System.Collections.Generic;

namespace Super.Model.Selection.Adapters
{
	public class SequenceSelector<_, T> : Selector<_, IEnumerable<T>>
	{
		public SequenceSelector(ISelect<_, IEnumerable<T>> subject) : base(subject) {}
	}
}