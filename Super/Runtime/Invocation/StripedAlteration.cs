using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;

namespace Super.Runtime.Invocation
{
	sealed class StripedAlteration<TIn, TOut> : DelegatedAlteration<ISelect<TIn, TOut>>
	{
		public static StripedAlteration<TIn, TOut> Default { get; } = new StripedAlteration<TIn, TOut>();

		StripedAlteration() : base(I<Striped<TIn, TOut>>.Default.From) {}
	}
}