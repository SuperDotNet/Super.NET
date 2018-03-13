using System.Collections;
using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Collections
{
	class Elements<T> : Membership<T>, IElements<T>
	{
		readonly IEnumerable<T> _items;

		public Elements(ICollection<T> collection) : this(collection, new AddCommand<T>(collection),
		                                                  new RemoveCommand<T>(collection)) {}

		public Elements(IEnumerable<T> items, ICommand<T> add, ICommand<T> remove) : base(add, remove) => _items = items;

		public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}