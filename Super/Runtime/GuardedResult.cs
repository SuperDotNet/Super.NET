using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Text;
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

	sealed class DefaultGuard<T> : AssignedGuard<T>
	{
		public static DefaultGuard<T> Default { get; } = new DefaultGuard<T>();

		DefaultGuard() : base(DefaultMessage.Default) {}
	}

	class AssignedGuard<T> : GuardedSpecification<T, InvalidOperationException>, IActivateMarker<ISelect<T, string>>
	{
		public AssignedGuard(Func<Type, string> message) : this(new Message<Type>(message)) {}

		public AssignedGuard(ISelect<Type, string> message) : this(IsAssigned<T>.Default, message) {}

		public AssignedGuard(ISpecification<T> specification, ISelect<Type, string> message)
			: base(specification,
			       message.New(I<InvalidOperationException>.Default)
			              .Out(Type<T>.Instance)
			              .Out(I<T>.Default)
			              .ToDelegate()) {}
	}
}