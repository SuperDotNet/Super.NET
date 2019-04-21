using Super.Model.Selection;

namespace Super.Model.Sequences.Query
{
	public interface IElement<T> : IElement<T, T> {}

	public interface IElement<TFrom, out TTo> : ISelect<ArrayView<TFrom>, TTo> {}
}