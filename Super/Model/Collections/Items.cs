using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Model.Collections
{
	public class Items<T> : ItemsBase<T>
	{
		readonly ImmutableArray<T> _items;
		readonly int               _length;

		public Items(IEnumerable<T> items) : this(items.ToImmutableArray()) {}

		public Items(params T[] items) : this(items.ToImmutableArray()) {}

		public Items(ImmutableArray<T> items) : this(items, items.Length) {}

		public Items(ImmutableArray<T> items, int length)
		{
			_items  = items;
			_length = length;
		}

		public sealed override IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < _length; i++)
			{
				yield return _items[i];
			}
		}
	}
}