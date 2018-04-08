using Super.Model.Commands;

namespace Super.Model.Selection
{
	public interface IMutable<TParameter, TResult> : ISelect<TParameter, TResult>, IAssignable<TParameter, TResult> {}
}
