using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class Lookup<TParameter, TResult> : ISpecification<TParameter, TResult>, IActivateMarker<IReadOnlyDictionary<TParameter, TResult>>
	{
		readonly IReadOnlyDictionary<TParameter, TResult> _store;

		public Lookup(IReadOnlyDictionary<TParameter, TResult> store) => _store = store;

		public bool IsSatisfiedBy(TParameter parameter) => _store.ContainsKey(parameter);

		public TResult Get(TParameter parameter) =>
			_store.TryGetValue(parameter, out var result) ? result : default;
	}
}
