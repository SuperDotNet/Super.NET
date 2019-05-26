using Super.Model.Sequences;

namespace Super.Model.Selection.Stores
{
	public interface ITableValues<TIn, TOut> : ITable<TIn, TOut>
	{
		IArray<TOut> Values { get; }
	}
}