using Super.Model.Sources;
using Super.Runtime.Activation;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Collections
{
	public class Items<T> : /*ItemsBase<T>,*/ Source<IEnumerable<T>>, IItems<T>, IActivateMarker<IEnumerable<T>>, IEnumerable<T>
	{
		readonly ImmutableArray<T> _items;
		readonly int _length;

		public Items(params T[] items) : this(items.AsEnumerable()) {}

		public Items(IEnumerable<T> items) : this(items.ToImmutableArray()) {}

		public Items(ImmutableArray<T> items) : this(items, items.Length) {}

		public Items(ImmutableArray<T> items, int length) : base(items.ToArray())
		{
			_items = items;
			_length = length;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < _length; i++)
			{
				yield return _items[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}