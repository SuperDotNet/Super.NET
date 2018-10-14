using System.Collections.Generic;
using Super.Model.Selection.Stores;

namespace Super.Model.Collections.Groups
{
	public class GroupNames : TableValues<string, GroupName>
	{
		public GroupNames(params GroupName[] names) : this(names.ToOrderedDictionary(x => x.Name)) {}

		public GroupNames(IDictionary<string, GroupName> store) : base(store) {}
	}
}