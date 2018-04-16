using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public class GroupCollection<T> : Select<GroupName, IList<T>>, IGroupCollection<T>
	{
		readonly Func<IEnumerable<T>, IEnumerable<T>>    _select;
		readonly IOrderedDictionary<GroupName, IList<T>> _store;

		public GroupCollection(IEnumerable<IGroup<T>> groups)
			: this(groups, GroupPairs<T>.Default) {}

		public GroupCollection(IEnumerable<IGroup<T>> groups, IGroupPairs<T> pairs)
			: this(new OrderedDictionary<GroupName, IList<T>>(groups.Select(pairs.Get)), SortAlteration<T>.Default.Get) {}

		public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store, Func<IEnumerable<T>, IEnumerable<T>> select) :
			base(store.GetValue)
		{
			_store  = store;
			_select = select;
		}

		public IEnumerator<T> GetEnumerator() => _store.Select(x => x.Value)
		                                               .Select(_select)
		                                               .Concat()
		                                               .GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}