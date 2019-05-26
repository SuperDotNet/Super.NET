using System.Collections.Generic;
using Super.Model.Selection;

namespace Super.Model.Sequences
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, Store<T>> {}
}