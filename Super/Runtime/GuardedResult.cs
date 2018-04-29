using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime
{
	sealed class DefaultGuard<T> : AssignedGuard<T>
	{
		public static DefaultGuard<T> Default { get; } = new DefaultGuard<T>();

		DefaultGuard() : base(DefaultMessage.Default) {}
	}

	class AssignedGuard<T> : Guard<T>
	{
		public AssignedGuard(Func<Type, string> message) : this(message.Out()) {}

		public AssignedGuard(ISelect<Type, string> message) : this(message.Out(Type<T>.Instance).Out(I<T>.Default)) {}

		public AssignedGuard(ISelect<T, string> message) : base(IsAssigned<T>.Default, message) {}
	}

	class Guard<T> : GuardedSpecification<T, InvalidOperationException>, IActivateMarker<ISelect<T, string>>
	{
		public Guard(ISpecification<T> specification, ISelect<T, string> message)
			: base(specification, message.New(I<InvalidOperationException>.Default).Get) {}
	}
}