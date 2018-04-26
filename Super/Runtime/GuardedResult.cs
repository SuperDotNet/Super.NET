using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime
{
	/*sealed class GuardedResult<TParameter, TResult> : Validated<TParameter, TResult>
	{
		public static GuardedResult<TParameter, TResult> Default { get; } = new GuardedResult<TParameter, TResult>();

		GuardedResult() : this(DefaultMessage<TParameter, TResult>.Default) {}

		public GuardedResult(ISelect<TParameter, string> message)
			: base(new AssignedInstanceGuard<TParameter>(Never<TParameter>.Default, message),
			       Default<TParameter, TResult>.Instance) {}
	}*/

	/*sealed class GuardedResult<T> : ValidatedSource<T>
	{
		public static GuardedResult<T> Default { get; } = new GuardedResult<T>();

		GuardedResult() : this(DefaultMessage<T>.Default) {}

		public GuardedResult(ISelect<T, string> message)
			: base(new Guard<T>(Never<T>.Default, message),
			       Default<TParameter, TResult>.Instance) {}
	}*/

	sealed class Guard<T> : GuardedSpecification<T, InvalidOperationException>, IActivateMarker<ISelect<T, string>>
	{
		public static Guard<T> Default { get; } = new Guard<T>();

		Guard() : this(DefaultMessage<T>.Default) {}

		public Guard(ISelect<T, string> message) : this(IsAssigned<T>.Default, message) {}

		public Guard(ISpecification<T> specification, ISelect<T, string> message)
			: this(specification, message.New(I<InvalidOperationException>.Default).Get) {}

		public Guard(ISpecification<T> specification, Func<T, InvalidOperationException> exception)
			: base(specification, exception) {}
	}
}