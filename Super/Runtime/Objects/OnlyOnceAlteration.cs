using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;
using Super.Runtime.Execution;

namespace Super.Runtime.Objects
{
	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new First().Out()
			              .ToSelect(I.A<TIn>())
			              .To(parameter.If);
	}
}