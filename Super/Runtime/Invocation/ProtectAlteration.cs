using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;

namespace Super.Runtime.Invocation
{
	sealed class ProtectAlteration<TParameter, TResult> : DelegatedAlteration<ISelect<TParameter, TResult>>
	{
		public static ProtectAlteration<TParameter, TResult> Default { get; } = new ProtectAlteration<TParameter, TResult>();

		ProtectAlteration() : base(I<Protect<TParameter, TResult>>.Default.From) {}
	}
}