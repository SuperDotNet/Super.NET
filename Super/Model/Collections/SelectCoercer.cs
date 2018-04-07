using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	class SelectCoercer<TFrom, TTo> : ISource<IEnumerable<TFrom>, IEnumerable<TTo>>, IActivateMarker<Func<TFrom, TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectCoercer(Func<TFrom, TTo> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.Select(_select);
	}

	class SelectManyCoercer<TFrom, TTo> : ISource<IEnumerable<TFrom>, IEnumerable<TTo>>, IActivateMarker<Func<TFrom, IEnumerable<TTo>>>
	{
		readonly Func<TFrom, IEnumerable<TTo>> _select;

		public SelectManyCoercer(Func<TFrom, IEnumerable<TTo>> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.SelectMany(_select);
	}
}