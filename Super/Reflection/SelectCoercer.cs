using System;
using System.Collections.Immutable;
using System.Linq;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class SelectCoercer<TFrom, TTo> : ISource<ImmutableArray<TFrom>, ImmutableArray<TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectCoercer(Func<TFrom, TTo> select) => _select = select;

		public ImmutableArray<TTo> Get(ImmutableArray<TFrom> parameter) => parameter.Select(_select).ToImmutableArray();
	}
}