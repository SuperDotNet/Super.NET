using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Text;

namespace Super.Runtime
{
	class GuardedFallback<TParameter, TResult> : DelegatedConditional<TParameter, TResult>
	{
		public static GuardedFallback<TParameter, TResult> Default { get; } = new GuardedFallback<TParameter, TResult>();

		GuardedFallback() : this(DefaultMessage<TParameter, TResult>.Default) {}

		public GuardedFallback(IMessage<TParameter> message)
			: base(new AssignedInstanceGuard<TParameter>(NeverSpecification<TParameter>.Default, message),
			       Default<TParameter, TResult>.Instance) {}
	}
}