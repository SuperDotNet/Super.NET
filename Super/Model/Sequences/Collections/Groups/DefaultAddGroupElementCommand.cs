using Super.Model.Commands;
using Super.Model.Selection.Conditions;

namespace Super.Model.Sequences.Collections.Groups
{
	class DefaultAddGroupElementCommand<T> : Command<T>
	{
		public DefaultAddGroupElementCommand(GroupName defaultName, IConditional<string, GroupName> names,
		                                     IGroupCollection<T> collection)
			: base(new AddGroupElementCommand<T>(collection, new GroupName<T>(defaultName, names))
			       .ToSelect()
			       .UnlessIsOf(new GroupingAwareCommand<T>(collection).ToSelect())
			       .ToCommand()) {}
	}
}