using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public interface IIterate<T> : ISelect<IEnumerable<T>, Store<T>> {}
}