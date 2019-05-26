using Super.Compose;
using Super.Model.Selection;

namespace Super.Aspects
{
	public sealed class EmptyAspect<TIn, TOut> : Select<ISelect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public static EmptyAspect<TIn, TOut> Default { get; } = new EmptyAspect<TIn, TOut>();

		EmptyAspect() : base(A.Self<ISelect<TIn, TOut>>()) {}
	}
}