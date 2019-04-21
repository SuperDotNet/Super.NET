using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;

namespace Super.Runtime.Invocation
{
	sealed class ProtectAlteration<TIn, TOut> : DelegatedAlteration<ISelect<TIn, TOut>>
	{
		public static ProtectAlteration<TIn, TOut> Default { get; } = new ProtectAlteration<TIn, TOut>();

		ProtectAlteration() : base(I<Protect<TIn, TOut>>.Default.From) {}
	}
}