using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Runtime
{
	class GuardedFallback<TParameter, TResult> : Conditional<TParameter, TResult>
	{
		public static GuardedFallback<TParameter, TResult> Default { get; } = new GuardedFallback<TParameter, TResult>();

		GuardedFallback() : this(Message<TParameter, TResult>.Default) {}

		public GuardedFallback(IMessage<TParameter> message)
			: base(new AssignedInstanceGuard<TParameter>(NeverSpecification<TParameter>.Default, message),
			       Default<TParameter, TResult>.Instance) {}
	}
}