using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Runtime.Execution;

namespace Super.Runtime.Objects
{
	sealed class OncePerParameter<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OncePerParameter<TIn, TOut> Default { get; } = new OncePerParameter<TIn, TOut>();

		OncePerParameter() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => parameter.If(new First<TIn>());
	}
}