using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections.Groups
{
	public class GroupCollection<T> : Select<GroupName, IList<T>>, IGroupCollection<T>
	{
		readonly IArray<T> _values;

		public GroupCollection(IEnumerable<IGroup<T>> groups) : this(groups, GroupPairs<T>.Default) {}

		public GroupCollection(IEnumerable<IGroup<T>> groups, IGroupPairs<T> pairs)
			: this(new OrderedDictionary<GroupName, IList<T>>(groups.Select(pairs.Get))) {}

		public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store)
			: this(store,
			       store.Select(x => x.Value.ToArray())
			            .Select(SortAlteration<T>.Default.Get)
			            .Concat()
			            .To(I<DeferredArray<T>>.Default)) {}

		public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store, IArray<T> values)
			: base(store.GetValue) => _values = values;

		public Array<T> Get() => _values.Get();
	}
}