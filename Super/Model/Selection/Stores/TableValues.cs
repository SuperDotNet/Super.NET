using Super.Model.Sequences;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class TableValues<TParameter, TResult> : Specification<TParameter, TResult>, IArray<TResult>
	{
		readonly IArray<TResult> _items;

		public TableValues(IDictionary<TParameter, TResult> store)
			: this(new Table<TParameter, TResult>(store), new DeferredArray<TResult>(store.Values)) {}

		public TableValues(ISpecification<TParameter, TResult> source, IArray<TResult> items)
			: base(source.IsSatisfiedBy, source.Get) => _items = items;

		public Array<TResult> Get() => _items.Get();
	}
}