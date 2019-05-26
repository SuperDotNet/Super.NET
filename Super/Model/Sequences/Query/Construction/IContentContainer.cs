using Super.Model.Selection;

namespace Super.Model.Sequences.Query.Construction
{
	public interface IContentContainer<TIn, TOut> : ISelect<IStores<TOut>, Assigned<uint>, IContent<TIn, TOut>> {}
}