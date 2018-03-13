using System;
using System.Collections.Generic;
using System.Linq;
using Super.Runtime;

namespace Super.Model.Collections
{
	// ATTRIBUTION: https://msdn.microsoft.com/en-us/library/ms404549%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
	public class KeyedByTypeCollection<T> : DelegatedKeyedCollection<Type, T>
	{
		public KeyedByTypeCollection() : this(Enumerable.Empty<T>()) {}

		public KeyedByTypeCollection(IEnumerable<T> items) : base(InstanceTypeCoercer<T>.Default.Get)
		{
			foreach (var obj in items)
			{
				Add(obj);
			}
		}

		protected sealed override void InsertItem(int index, T item)
		{
			if (Contains(item.GetType()))
			{
				throw new InvalidOperationException($"Duplicate type: {item.GetType().FullName}");
			}

			base.InsertItem(index, item);
		}

		protected sealed override void SetItem(int index, T item)
		{
			base.SetItem(index, item);
		}
	}
}