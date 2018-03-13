using System;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Specifications
{
	sealed class AssignedInstanceGuard<T> : GuardedSpecification<T, InvalidOperationException>
	{
		public AssignedInstanceGuard(ISpecification<T> specification, IMessage<T> message)
			: this(specification, Exceptions.From(x => new InvalidOperationException(x)).In(message).Get) {}

		public AssignedInstanceGuard(ISpecification<T> specification, Func<T, InvalidOperationException> exception) :
			base(specification, exception) {}
	}
}