using Super.Model.Selection;
using Super.Model.Specifications;

namespace Super.Runtime
{
	sealed class GuardedFallback<TParameter, TResult> : DelegatedConditional<TParameter, TResult>
	{
		public static GuardedFallback<TParameter, TResult> Default { get; } = new GuardedFallback<TParameter, TResult>();

		GuardedFallback() : this(DefaultMessage<TParameter, TResult>.Default) {}

		public GuardedFallback(ISelect<TParameter, string> message)
			: base(new AssignedInstanceGuard<TParameter>(Never<TParameter>.Default, message),
			       Default<TParameter, TResult>.Instance) {}
	}
}