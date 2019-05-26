using Super.Model.Selection;

namespace Super.Model.Sequences.Query.Construction
{
	public interface IContents<TIn, TOut> : ISelect<Parameter<TIn, TOut>, IContent<TIn, TOut>> {}
}