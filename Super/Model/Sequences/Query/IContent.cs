using Super.Model.Selection;

namespace Super.Model.Sequences.Query
{
	public interface IContent<TIn, TOut> : ISelect<Store<TIn>, Store<TOut>> {}
}