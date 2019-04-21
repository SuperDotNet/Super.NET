using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime
{
	sealed class DefaultGuard<T> : AssignedGuard<T>
	{
		public static DefaultGuard<T> Default { get; } = new DefaultGuard<T>();

		DefaultGuard() : base(DefaultMessage.Default.Get) {}
	}

	class AssignedGuard<T> : Guard<T>, IActivateUsing<ISelect<T, string>>
	{
		public AssignedGuard(Func<Type, string> message) : this(message.ToSelect().In(Type<T>.Instance).ToSelect(I.A<T>())) {}

		public AssignedGuard(ISelect<T, string> message) : base(IsAssigned<T>.Default, message) {}
	}

	class Guard<T> : GuardedCondition<T, InvalidOperationException>
	{
		public Guard(ICondition<T> condition, ISelect<T, string> message)
			: base(condition, Start.A.Selection<string>()
			                       .AndOf<InvalidOperationException>()
			                       .By.Instantiation.To(message.Select)
			                       .Get) {}
	}
}