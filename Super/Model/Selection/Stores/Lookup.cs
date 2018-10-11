using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class Lookup<TParameter, TResult> : ISpecification<TParameter, TResult>,
	                                           IActivateMarker<IReadOnlyDictionary<TParameter, TResult>>,
	                                           IActivateMarker<IDictionary<TParameter, TResult>>
	{
		readonly IReadOnlyDictionary<TParameter, TResult> _store;
		readonly TResult _default;

		public Lookup(IDictionary<TParameter, TResult> dictionary) : this(dictionary.AsReadOnly()) {}

		public Lookup(IReadOnlyDictionary<TParameter, TResult> store, TResult @default = default)
		{
			_store = store;
			_default = @default;
		}

		public bool IsSatisfiedBy(TParameter parameter) => _store.ContainsKey(parameter);

		public TResult Get(TParameter parameter) => _store.TryGetValue(parameter, out var result) ? result : _default;
	}
}
