using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Reflection;

namespace Super.Runtime.Invocation
{
	sealed class StripedAlteration<TParameter, TResult> : DelegatedAlteration<ISelect<TParameter, TResult>>
	{
		public static StripedAlteration<TParameter, TResult> Default { get; } = new StripedAlteration<TParameter, TResult>();

		StripedAlteration() : base(I<Striped<TParameter, TResult>>.Default.From) {}
	}
}