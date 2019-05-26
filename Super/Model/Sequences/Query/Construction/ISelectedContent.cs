using Super.Model.Selection;

namespace Super.Model.Sequences.Query.Construction
{
	public interface ISelectedContent<TIn, TOut> : ISelect<Assigned<uint>, IContent<TIn, TOut>> {}
}