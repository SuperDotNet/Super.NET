using System.Collections.Generic;
using System.Linq;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	class OfTypeCoercer<TFrom, TTo> : ISource<IEnumerable<TFrom>, IEnumerable<TTo>> where TTo : TFrom
	{
		public static OfTypeCoercer<TFrom, TTo> Default { get; } = new OfTypeCoercer<TFrom, TTo>();

		OfTypeCoercer() {}

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.OfType<TTo>();
	}
}