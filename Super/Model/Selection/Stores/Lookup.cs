using Super.Compose;
using Super.Model.Selection.Conditions;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class Lookup<TIn, TOut> : Conditional<TIn, TOut>,
	                                 IActivateUsing<IReadOnlyDictionary<TIn, TOut>>,
	                                 IActivateUsing<IDictionary<TIn, TOut>>
	{
		public Lookup(IDictionary<TIn, TOut> dictionary) : this(dictionary.AsReadOnly()) {}

		public Lookup(IReadOnlyDictionary<TIn, TOut> store) : this(store, Start.A.Selection<TIn>()
		                                                                       .By.Default<TOut>()
		                                                                       .Get) {}

		public Lookup(IReadOnlyDictionary<TIn, TOut> store, Func<TIn, TOut> @default)
			: base(store.ContainsKey, new TableValueAdapter<TIn, TOut>(store, @default).Get) {}
	}

	sealed class TableValueAdapter<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly IReadOnlyDictionary<TIn, TOut> _store;
		readonly Func<TIn, TOut>                _default;

		public TableValueAdapter(IReadOnlyDictionary<TIn, TOut> store, Func<TIn, TOut> @default)
		{
			_store   = store;
			_default = @default;
		}

		public TOut Get(TIn parameter) => _store.TryGetValue(parameter, out var result) ? result : _default(parameter);
	}
}