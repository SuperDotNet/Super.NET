using Super.Model.Commands;
using Super.Model.Selection;

namespace Super.Model.Collections.Groups
{
	class DefaultAddGroupElementCommand<T> : DecoratedCommand<T>
	{
		public DefaultAddGroupElementCommand(GroupName defaultName, ISpecification<string, GroupName> names,
		                                     IGroupCollection<T> collection)
			: base(new AddGroupElementCommand<T>(collection, new GroupName<T>(defaultName, names))
			       .Out()
			       .UnlessCast(new GroupingAwareCommand<T>(collection).Out())
			       .Out()) {}
	}
}