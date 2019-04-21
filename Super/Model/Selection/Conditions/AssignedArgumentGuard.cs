using System;
using Super.Runtime;

namespace Super.Model.Selection.Conditions
{
	sealed class AssignedArgumentGuard<T> : GuardedCondition<T, ArgumentNullException>
	{
		public static AssignedArgumentGuard<T> Default { get; } = new AssignedArgumentGuard<T>();

		AssignedArgumentGuard() : this(IsAssigned<T>.Default,
		                               new ArgumentNullException($"Argument of type {typeof(T)} was not assigned.")) {}

		public AssignedArgumentGuard(ICondition<T> condition, ArgumentNullException exception)
			: base(condition, exception.Accept) {}
	}
}