using Super.Model.Selection;

namespace Super.Model.Sequences.Query.Construction
{
	public interface IBodyBuilder<T> : ISelect<Partitioning, IBody<T>> {}
}