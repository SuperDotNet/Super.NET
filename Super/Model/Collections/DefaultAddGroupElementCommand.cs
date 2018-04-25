using Super.Model.Commands;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	class DefaultAddGroupElementCommand<T> : DecoratedCommand<T>
	{
		public DefaultAddGroupElementCommand(GroupName defaultName, ISpecification<string, GroupName> names,
		                                     IGroupCollection<T> collection)
			: base(new AddGroupElementCommand<T>(collection, new GroupName<T>(defaultName, names))
			       .Start()
			       .Unless(new GroupingAwareCommand<T>(collection).Start())
			       .ToCommand()) {}
	}
}