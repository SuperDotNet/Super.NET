using Super.Model.Commands;

namespace Super.Model.Selection
{
	public interface IMutable<TIn, TOut> : ISelect<TIn, TOut>, IAssign<TIn, TOut> {}
}
