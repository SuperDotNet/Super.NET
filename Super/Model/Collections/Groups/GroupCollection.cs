using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections.Groups
{
	public class GroupCollection<T> : Selection.Select<GroupName, IList<T>>, IGroupCollection<T>
	{
		public GroupCollection(IEnumerable<IGroup<T>> groups) : this(groups, GroupPairs<T>.Default) {}

		public GroupCollection(IEnumerable<IGroup<T>> groups, IGroupPairs<T> pairs)
			: this(groups.Select(pairs.Get).ToOrderedDictionary()) {}

		public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store)
			: this(store,
			       store.Select(x => x.Value.ToArray())
			            .Select(SortAlteration<T>.Default.Get)
			            .SelectMany(x => x)
			            .To(I<DeferredArray<T>>.Default)) {}

		public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store, IArray<T> values)
			: base(store.GetValue) => Values = values;

		public IArray<T> Values { get; }
	}
}