using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Model.Sequences.Query.Construction
{
	public interface IPartition : IAlteration<Selection> {}

	public interface IPartition<T> : ISelect<Assigned<uint>, IBody<T>>, ISelect<IPartition, IPartition<T>> {}
}