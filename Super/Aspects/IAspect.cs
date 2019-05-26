using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Aspects
{
	public interface IAspect<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}
}