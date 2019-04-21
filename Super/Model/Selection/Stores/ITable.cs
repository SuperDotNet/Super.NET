using Super.Model.Commands;
using Super.Model.Selection.Conditions;

namespace Super.Model.Selection.Stores
{
	public interface ITable<TIn, TOut> : IConditional<TIn, TOut>, IAssign<TIn, TOut>
	{
		bool Remove(TIn key);
	}
}