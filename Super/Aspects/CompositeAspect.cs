using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;

namespace Super.Aspects
{
	public class CompositeAspect<TIn, TOut> : Aggregate<IAspect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public CompositeAspect(params IAspect<TIn, TOut>[] aspects) : this(new Array<IAspect<TIn, TOut>>(aspects)) {}

		public CompositeAspect(Array<IAspect<TIn, TOut>> items) : base(items) {}
	}
}