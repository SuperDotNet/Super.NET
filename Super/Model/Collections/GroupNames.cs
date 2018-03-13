using System.Collections.Generic;
using Super.ExtensionMethods;
using Super.Model.Sources.Tables;

namespace Super.Model.Collections
{
	public class GroupNames : TableValues<string, GroupName>
	{
		public GroupNames(params GroupName[] names) : this(names.ToOrderedDictionary(x => x.Name)) {}

		public GroupNames(IDictionary<string, GroupName> store) : base(store) {}
	}
}