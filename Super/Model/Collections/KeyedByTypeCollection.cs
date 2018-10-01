using Super.Runtime;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	/// <summary>
	/// ATTRIBUTION: https://msdn.microsoft.com/en-us/library/ms404549%28v=vs.110%29.aspx?f=255&amp;MSPPError=-2147217396
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyedByTypeCollection<T> : DelegatedKeyedCollection<Type, T>
	{
		public KeyedByTypeCollection() : this(Empty<T>.Enumerable) {}

		public KeyedByTypeCollection(IEnumerable<T> items) : base(InstanceTypeSelector<T>.Default.Get)
		{
			foreach (var obj in items)
			{
				Add(obj);
			}
		}

		protected sealed override void InsertItem(int index, T item)
		{
			var key = GetKeyForItem(item);
			if (Contains(key))
			{
				Dictionary.Remove(key);
			}
			base.InsertItem(index, item);
		}
	}
}