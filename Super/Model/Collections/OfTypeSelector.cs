using Super.Model.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	class OfTypeSelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> where TTo : TFrom
	{
		public static OfTypeSelector<TFrom, TTo> Default { get; } = new OfTypeSelector<TFrom, TTo>();

		OfTypeSelector() {}

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.OfType<TTo>();
	}
}