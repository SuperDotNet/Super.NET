using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	class DefaultAddGroupElementCommand<T> : DecoratedCommand<T>
	{
		public DefaultAddGroupElementCommand(GroupName defaultName, ISpecification<string, GroupName> names,
		                                     IGroupCollection<T> collection)
			: base(new AddGroupElementCommand<T>(collection, new GroupName<T>(defaultName, names))
			       .Adapt()
			       .Unless(new GroupingAwareCommand<T>(collection).Adapt())
			       .ToCommand()) {}
	}
}