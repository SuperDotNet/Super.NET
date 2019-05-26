using Super.Compose;
using Super.Model.Selection;

namespace Super.Model.Sequences.Collections.Groups
{
	class MetadataGroupName<T> : Select<T, GroupName>, IGroupName<T>
	{
		public MetadataGroupName(ISelect<string, GroupName> names) : base(Start.An.Instance<DeclaredGroupNames<T>>()
		                                                                       .Select(names)
		                                                                       .Get) {}
	}
}