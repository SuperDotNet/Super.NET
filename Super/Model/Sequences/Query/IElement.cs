using Super.Model.Selection;

namespace Super.Model.Sequences.Query
{
	public interface IElement<T> : ISelect<ArrayView<T>, T> {}
}